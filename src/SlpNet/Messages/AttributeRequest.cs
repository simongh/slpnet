using System.Collections.Generic;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Allows a UA to discover the attributes for a service or service-type
	/// </summary>
	internal class AttributeRequest : RequestBase
	{
		protected override int FunctionId
		{
			get { return 6; }
		}

		/// <summary>
		/// Gets the list of previous IP addresses
		/// </summary>
		public List<System.Net.IPAddress> PreviousResponders
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets either the URL or service-type to query
		/// </summary>
		public ServiceUri Uri
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of scopes to use
		/// </summary>
		public List<string> Scopes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of tags to return. Leave blank for all tags
		/// </summary>
		public List<string> Tags
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the SPI string
		/// </summary>
		public string Spi
		{
			get;
			set;
		}

		public AttributeRequest()
			:base()
		{
			Scopes = new List<string>();
			Tags = new List<string>();
			PreviousResponders = new List<System.Net.IPAddress>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			WriteIPList(innerwriter, PreviousResponders);
			innerwriter.Write(Uri.ToString());
			innerwriter.Write(Scopes);
			innerwriter.Write(Tags);
			innerwriter.Write(Spi);

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			ReadIPList(reader, PreviousResponders);
			Uri = new ServiceUri(reader.ReadString());
			Scopes.AddRange(reader.ReadList());
			Tags.AddRange(reader.ReadList());
			Spi = reader.ReadString();
		}
	}
}
