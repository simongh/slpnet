
using System.Collections.Generic;
namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Registers a service with a DA
	/// </summary>
	internal class ServiceRegistrationRequest : RequestBase
	{
		protected override int FunctionId
		{
			get { return 3; }
		}

		/// <summary>
		/// Gets the service to register
		/// </summary>
		public ServiceEntry Service
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of scopes
		/// </summary>
		public List<string> Scopes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of authentication blocks
		/// </summary>
		public List<Security.AuthenticationBlock> AuthBlocks
		{
			get;
			private set;
		}

		public ServiceRegistrationRequest(ServiceEntry service)
			: this()
		{
			Service = service;
		}

		public ServiceRegistrationRequest()
		{
			Scopes = new List<string>();
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			Service.ToBytes(innerwriter);
			writer.Write(Service.Uri.ServiceType);
			innerwriter.Write(Scopes);
			Service.Uri.Attributes.ToBytes(innerwriter);
			WriteAuthBlocks(innerwriter, AuthBlocks);

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			Service = Services.Locator.GetInstance<ServiceEntry>(reader);
			var tmp = reader.ReadString();
			Scopes.AddRange(reader.ReadList());
			Service.Uri = new ServiceUri("service:" + tmp + ":" + Services.Locator.GetInstance<AttributeCollection>(reader));
			ReadAuthBlocks(reader, AuthBlocks);
		}
	}
}
