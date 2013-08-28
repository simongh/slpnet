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
		internal override ExtensionBase Create(SlpReader reader)
		{
			var id = reader.ReadInt16();
			var result = new UnknownExtension(id);
			result.Offset = reader.ReadInt24();
			result.Data = reader.ReadBytes(result.Offset);

			return result;
		}

		/// <summary>
		/// Converts this extension to a byte array
		/// </summary>
		/// <returns>array of bytes</returns>
		internal override void ToBytes(SlpWriter writer)
		{
			writer.Write((short)Id);
			writer.Write(Offset, 3);
			writer.Write(Data);
		}
	}
}
