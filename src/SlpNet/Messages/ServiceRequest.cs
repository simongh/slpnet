using System.Collections.Generic;
using System.Net;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Find services on the network
	/// </summary>
	internal class ServiceRequest : RequestBase
	{
		protected override int FunctionId
		{
			get { return 1; }
		}

		/// <summary>
		/// Gets the list of previous IP addresses
		/// </summary>
		public List<IPAddress> PreviousResponders
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the service-type to locate
		/// </summary>
		public string ServiceType
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
		/// Gets or sets the attribute filter. the filter uses LDAP syntax
		/// </summary>
		public string AttributeFilter
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SPI string
		/// </summary>
		public string Spi
		{
			get;
			set;
		}

		public ServiceRequest()
			: base()
		{
			Scopes = new List<string>();
			PreviousResponders = new List<IPAddress>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			WriteIPList(innerwriter, PreviousResponders);
			innerwriter.Write(ServiceType);
			innerwriter.Write(Scopes);
			innerwriter.Write(AttributeFilter);
			innerwriter.Write(Spi);

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			ReadIPList(reader, PreviousResponders);
			Scopes.AddRange(reader.ReadList());
			AttributeFilter = reader.ReadString();
			Spi = reader.ReadString();
		}
	}
}
