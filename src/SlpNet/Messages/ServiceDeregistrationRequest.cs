using System.Collections.Generic;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Allows an SA to deregister its service with the DA
	/// </summary>
	internal class ServiceDeregistrationRequest : RequestBase
	{
		protected override int FunctionId
		{
			get { return 4; }
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
		/// Gets or sets the service to deregister
		/// </summary>
		public ServiceEntry Service
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of tags to deregister. Leave blank to fully deregister the service
		/// </summary>
		public List<string> Tags
		{
			get;
			private set;
		}

		public ServiceDeregistrationRequest()
			: base()
		{
			Scopes = new List<string>();
			Tags = new List<string>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			innerwriter.Write(Scopes);
			Service.ToBytes(innerwriter);
			innerwriter.Write(Tags);

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			Scopes.AddRange(reader.ReadList());
			Service = Services.Locator.GetInstance<ServiceEntry>(reader);
			Tags.AddRange(reader.ReadList());
		}
	}
}
