
using System;
using System.Collections.Generic;
namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Directory Agent Advert sent to indicate the prescence of a DA
	/// </summary>
	internal class DirectoryAgentAdvert : ReplyBase
	{
		protected override int FunctionId
		{
			get { return 8; }
		}

		/// <summary>
		/// Gets or sets the time the DA started
		/// </summary>
		public DateTime BootTimeStamp
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the service url for the DA
		/// </summary>
		public ServiceUri Uri
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of scopes this DA supports
		/// </summary>
		public List<string> Scopes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list of SPIs this DA can verify
		/// </summary>
		public List<string> SpiStrings
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

		public DirectoryAgentAdvert(ServiceUri uri)
			: this()
		{
			if (uri.ServiceType != "directory-agent")
				throw new ArgumentException("A directory-agent service URI is the only allowed value.");
			Uri = uri;
		}

		public DirectoryAgentAdvert()
			: base()
		{
			Scopes = new List<string>();
			SpiStrings = new List<string>();
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			innerwriter.Write((short)ErrorCode);
			innerwriter.Write(BootTimeStamp);
			innerwriter.Write("service:directory-agent://" + Uri.Host);
			innerwriter.Write(Scopes);
			Uri.Attributes.ToBytes(innerwriter);
			innerwriter.Write(SpiStrings);
			WriteAuthBlocks(innerwriter, AuthBlocks);

			writer.Write(innerwriter.Length, 3);

			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			ErrorCode = (ServiceErrorCode)reader.ReadInt16();
			BootTimeStamp = reader.ReadDateTime();
			var url = reader.ReadString();
			Scopes.AddRange(reader.ReadList());
			Uri = new ServiceUri(url, Services.Locator.GetInstance<AttributeCollection>(reader));
			SpiStrings.AddRange(reader.ReadList());
			ReadAuthBlocks(reader, AuthBlocks);
		}
	}
}
