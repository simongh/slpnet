using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Discovery.Slp
{
	public class SlpWriter
	{
		private readonly Stream _stream;
		private readonly ASCIIEncoding _encoder;

		public int Length
		{
			get;
			private set;
		}

		public SlpWriter(Stream output)
		{
			_encoder = new ASCIIEncoding();
			_stream = output;
		}

		public void Write(byte[] buffer)
		{
			_stream.Write(buffer, 0, buffer.Length);
			Length += buffer.Length;
		}

		public void Write(byte value)
		{
			_stream.WriteByte(value);
			Length++;
		}

		public void Write(int value)
		{
			Write(value, 4);
		}

		public void Write(int value, int count)
		{
			_stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, count);
			Length += count;
		}

		public void Write(short value)
		{
			Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
		}

		public void Write(string value)
		{
			Write(value, null);
		}

		public void Write(string value, string reserved)
		{
			if (value == null)
			{
				Write((short)0);
			}
			else
			{
				var buffer = GetBytes(value, reserved);
				Write((short)buffer.Length);
				Write(buffer);
			}
		}

		public void Write(IEnumerable<string> list)
		{
			Write(list, null);
		}

		public void Write(IEnumerable<string> list, string reserved)
		{
			var sb = new StringBuilder();
			foreach (var item in list)
			{
				if (sb.Length > 0)
					sb.Append(",");
				sb.Append(Escape(item, reserved));
			}

			WriteRaw(sb.ToString());
		}

		public void Write(DateTime value)
		{
			Write((int)value.Subtract(Constants.EPOCH).TotalSeconds);
		}

		public void Write(TimeSpan value)
		{
			Write((short)value.TotalSeconds);
		}

		public void Write(SlpWriter value)
		{
			value.CopyTo(_stream);
			Length += value.Length;
		}

		public void WriteRaw(string value)
		{
			Write(value, null);
		}

		public void WriteTagList(IEnumerable<string> list)
		{
			WriteTagList(list, true);
		}

		public void WriteTagList(IEnumerable<string> list, bool allowWildcards)
		{
			Write(TagListEncode(list, allowWildcards));
		}

		public byte[] GetBytes(string value)
		{
			return GetBytes(value, null);
		}

		public byte[] GetBytes(string value, string reserved)
		{
			if (!string.IsNullOrEmpty(reserved))
				value = Escape(value, reserved);

			if (value == null)
			{
				return new byte[0];
			}
			else
			{
				return _encoder.GetBytes(value.Trim());
			}
		}

		public string Escape(string value, string reserved)
		{
			if (reserved == null || value == null)
				return value;

			return Regex.Replace(value, reserved, x => "\\" + char.GetNumericValue(x.Groups[1].Value, 0).ToString());
		}

		public string TagListEncode(IEnumerable<string> list)
		{
			return TagListEncode(list, true);
		}

		public string TagListEncode(IEnumerable<string> list, bool allowWildcards)
		{
			var tmp = string.Join(" ", list);

			if (!allowWildcards && tmp.Contains("*"))
				throw new ServiceException("Wildcards are not allowed in this attribute list.");

			return tmp;
		}

		private void CopyTo(Stream stream)
		{
			_stream.Seek(0, SeekOrigin.Begin);
			_stream.CopyTo(stream);
		}
	}
}
