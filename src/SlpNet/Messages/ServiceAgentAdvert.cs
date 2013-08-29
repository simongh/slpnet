using System;
using System.Collections.Generic;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Advertises the precence of a Service Agent
	/// </summary>
	internal class ServiceAgentAdvert : MessageBase
	{
		protected override int FunctionId
		{
			get { return 11; }
		}

		/// <summary>
		/// Gets the URI of this service agent
		/// </summary>
		public ServiceUri Uri
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of scopes this agent supports
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

		public ServiceAgentAdvert(ServiceUri uri)
			: this()
		{
			if (uri.ServiceType != "service-agent")
				throw new ArgumentException("A service-agent is the only allowable uri");
			Uri = uri;
		}

		public ServiceAgentAdvert()
			: base()
		{
			Scopes = new List<string>();
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			innerwriter.Write("service:service-agent://" + Uri.Host);
			innerwriter.Write(Scopes);
			Uri.Attributes.ToBytes(innerwriter);
			WriteAuthBlocks(innerwriter, AuthBlocks);

			writer.Write(innerwriter.Length, 3);

			WriteHeader(writer);
			writer.Write(innerwriter);

		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			var tmp = reader.ReadString();
			Scopes.AddRange(reader.ReadList());
			Uri = new ServiceUri(tmp, EntityFactory.Instance.CreateAttributeCollection(reader));
			ReadAuthBlocks(reader, AuthBlocks);
		}
	}
}
