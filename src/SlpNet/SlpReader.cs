using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Discovery.Slp
{
	public class SlpReader
	{
		private readonly Stream _stream;
		private readonly ASCIIEncoding _encoder;

		public SlpReader(Stream input)
		{
			_stream = input;
			_encoder = new ASCIIEncoding();
		}

		public byte[] ReadBytes(int count)
		{
			var buffer = new byte[count];
			_stream.Read(buffer, 0, count);

			return buffer;
		}

		public int ReadByte()
		{
			return _stream.ReadByte();
		}

		public short ReadInt16()
		{
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ReadBytes(2), 0));
		}

		public int ReadInt24()
		{
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ReadBytes(3), 0));
		}

		public int ReadInt32()
		{
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ReadBytes(4), 0));
		}

		public string ReadString()
		{
			return Unescape(ReadRawString());
		}

		public string ReadRawString()
		{
			var toread = ReadInt16();
			var buffer = ReadBytes(toread);

			return _encoder.GetString(buffer);
		}

		public IEnumerable<string> ReadList()
		{
			var tmp = ReadRawString();
			if (tmp == null)
				yield return null;

			foreach (var item in tmp.Split(','))
			{
				yield return Unescape(item);
			}
		}

		public TimeSpan ReadTimeSpan()
		{
			return new TimeSpan(0, 0, ReadInt16());
		}

		public DateTime ReadDateTime()
		{
			return Constants.EPOCH.AddSeconds(ReadInt32());
		}

		public string Unescape(string value)
		{
			if (value.StartsWith("\\FF", System.StringComparison.InvariantCultureIgnoreCase))
				return null;

			return Regex.Replace(value, Constants.VALIDTEXT, x => ((char)int.Parse("0x" + x.Groups[1].Value)).ToString());
		}

		public IEnumerable<string> TagListDecode()
		{
			return TagListDecode(true);
		}

		public IEnumerable<string> TagListDecode(bool allowWildcard)
		{
			var tmp = ReadString();

			if (!allowWildcard && tmp.Contains("*"))
				throw new ServiceException("Wildcards are not allowed this attribute list.");

			return tmp.Split(' ');
		}
	}
}
