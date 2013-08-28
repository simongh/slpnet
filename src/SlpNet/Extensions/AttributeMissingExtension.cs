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
		internal override ExtensionBase Create(SlpReader reader)
		{
			var result = new AttributeMissingExtension();

			Offset = reader.ReadInt24();
			TemplateName = reader.ReadString();
			Attributes.AddRange(reader.TagListDecode(false));

			return result;
		}

		/// <summary>
		/// Converts this extension to a byte array
		/// </summary>
		/// <returns>array of bytes</returns>
		internal override void ToBytes(SlpWriter writer)
		{
			writer.Write((short)Id);

			var tmp = writer.GetBytes(TemplateName + TemplateVersion + writer.TagListEncode(Attributes, false));
			writer.Write(tmp.Length, 3);
			writer.Write(tmp);
		}
	}
}
