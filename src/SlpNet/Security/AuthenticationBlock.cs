using System;

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

		internal void ToBytes(SlpWriter writer)
		{
			var spitmp = writer.GetBytes(SpiString);

			writer.Write((short)Descriptor);
			writer.Write((short)(2 + 2 + 4 + 2 + spitmp.Length + Data.Length));
			writer.Write(TimeStamp);
			writer.Write(SpiString);

			writer.Write(Data);
		}
	}
}
