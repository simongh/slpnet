
namespace Discovery.Slp.Extensions
{
	/// <summary>
	/// Base class for extensions
	/// </summary>
	public abstract class ExtensionBase
	{
		/// <summary>
		/// Gets the ID of this extension
		/// </summary>
		public abstract int Id
		{
			get;
		}

		/// <summary>
		/// Gets or sets the offset of the next extension
		/// </summary>
		internal protected int Offset
		{
			get;
			set;
		}

		/// <summary>
		/// Initalises a new extension from the received data
		/// </summary>
		/// <param name="data">byte array of data to parse</param>
		/// <returns>new Extension</returns>
		internal abstract ExtensionBase Create(SlpReader reader);

		/// <summary>
		/// Converts this extension to a byte array
		/// </summary>
		/// <returns>array of bytes</returns>
		internal abstract void ToBytes(SlpWriter writer);

	}
}
