using System.Collections.Generic;

namespace Discovery.Slp.Extensions
{
	/// <summary>
	/// Allows an SA or DA to re-request a message if required attributes where missing
	/// </summary>
	public class AttributeMissingExtension : ExtensionBase
	{
		/// <summary>
		/// Gets the ID of this extension
		/// </summary>
		public override int Id
		{
			get { return 0x0001; }
		}

		/// <summary>
		/// Gets or sets the service template name
		/// </summary>
		public string TemplateName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the service template version
		/// </summary>
		public string TemplateVersion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of missing attributes
		/// </summary>
		public List<string> Attributes
		{
			get;
			private set;
		}

		public AttributeMissingExtension()
			: base()
		{
			Attributes = new List<string>();
		}

		/// <summary>
		/// Initalises a new extension from the received data
		/// </summary>
		/// <param name="data">byte array of data to parse</param>
		/// <returns>new AttributeMissing Extension</returns>
		internal override ExtensionBase FromBytes(byte[] data)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream(data);

			var result = new AttributeMissingExtension();

			ms.Seek(2, System.IO.SeekOrigin.Begin);
			//result.Offset = Utilities.ReadInt(ms, 3);
			//result.TemplateName = Utilities.ReadString(ms);
			//result.Attributes = Utilities.TagListDecode(Utilities.ReadString(ms), false);

			return result;
		}

		/// <summary>
		/// Converts this extension to a byte array
		/// </summary>
		/// <returns>array of bytes</returns>
		internal override byte[] ToBytes()
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();

			//Utilities.WriteInt(ms, Id, 2);
			//Utilities.WriteInt(ms, 0, 3);
			//Utilities.WriteString(ms, TemplateName + TemplateVersion);
			//Utilities.WriteString(ms, Utilities.TagListEncode(Attributes, false));

			return ms.ToArray();
		}
	}
}
