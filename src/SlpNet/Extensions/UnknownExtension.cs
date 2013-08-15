using System;

namespace Discovery.Slp.Extensions
{
	public class UnknownExtension : ExtensionBase
	{
		private int _Id;

		public override int Id
		{
			get { return _Id; }
		}

		/// <summary>
		/// Gets or sets the payload for this extension
		/// </summary>
		internal byte[] Data
		{
			get;
			set;
		}

		public UnknownExtension(int id)
			: base()
		{
			if (id < 0 || id > 0x8fff)
				throw new ArgumentOutOfRangeException();
			_Id = id;
		}

		/// <summary>
		/// Initalises a new extension from the received data
		/// </summary>
		/// <param name="data">byte array of data to parse</param>
		/// <returns>new UserDefined Extension</returns>
		internal override ExtensionBase FromBytes(byte[] data)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream(data);

			var result = new UnknownExtension(this.Id);

			ms.Seek(2, System.IO.SeekOrigin.Begin);
			//result.Offset = Utilities.ReadInt(ms, 3);

			result.Data = new byte[Offset];
			ms.Read(result.Data, 0, result.Offset);

			return result;
		}

		/// <summary>
		/// Converts this extension to a byte array
		/// </summary>
		/// <returns>array of bytes</returns>
		internal override byte[] ToBytes()
		{
			byte[] buffer = new byte[Data.Length + 5];
			//Utilities.WriteInt(Id, 2).CopyTo(buffer, 0);

			Data.CopyTo(buffer, 5);
			return buffer;
		}
	}
}
