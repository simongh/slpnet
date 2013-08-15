using System;
using System.IO;

namespace Discovery.Slp.Security
{
	public class AuthenticationBlock
	{
		public const int DSA = 0x0002;

		public int Descriptor
		{
			get;
			set;
		}

		public DateTime TimeStamp
		{
			get;
			set;
		}

		public string SpiString
		{
			get;
			set;
		}

		public byte[] Data
		{
			get;
			set;
		}

		internal byte[] ToByteArray()
		{
			var ms = new MemoryStream();

			ToByteArray(ms);

			return ms.ToArray();
		}

		internal void ToByteArray(System.IO.Stream stream)
		{
			//Utilities.WriteInt(stream, Descriptor, 2);
			//Utilities.WriteInt(stream, 0, 2);
			//Utilities.WriteInt(stream, (int)TimeStamp.Subtract(SLP.Constants.EPOCH).TotalSeconds, 4);
			//Utilities.WriteString(stream, SpiString);

			//stream.Write(Data, 0, Data.Length);
			//stream.Seek(2, SeekOrigin.Begin);
			//Utilities.WriteInt((int)(stream.Length), 2);
		}

		internal static AuthenticationBlock Parse(System.IO.Stream stream)
		{
			var result = new AuthenticationBlock();
			//result.Descriptor = Utilities.ReadInt(stream, 2);
			//int len = Utilities.ReadInt(stream, 2);
			//result.TimeStamp = SLP.Constants.EPOCH.AddSeconds(Utilities.ReadInt(stream, 4));
			//result.SpiString = Utilities.ReadString(stream);

			//result.Data = new byte[len];
			//stream.Read(result.Data, 0, len);

			return result;
		}
	}
}
