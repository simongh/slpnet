using System.Collections.Generic;
using System.Net;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Allows a UA to discover serivces on the network
	/// </summary>
	internal class ServiceTypeRequest : RequestBase
	{
		protected override int FunctionId
		{
			get { return 9; }
		}

		/// <summary>
		/// Gets the list of previous SA responders
		/// </summary>
		public List<IPAddress> PreviousResponders
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the naming authority to find services for. Leave blank for IANA services
		/// </summary>
		public string NamingAuthority
		{
			get;
			set;
		}

		/// <summary>
		/// Gets whether the current naming authority is IANA
		/// </summary>
		public bool IsIANANamingAuthority
		{
			get { return string.IsNullOrEmpty(NamingAuthority); }
		}

		/// <summary>
		/// Gets or sets whether all naming authorities should be used
		/// </summary>
		public bool GetAllAuthorities
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of scopes
		/// </summary>
		public List<string> Scopes
		{
			get;
			private set;
		}

		public ServiceTypeRequest()
		{
			Scopes = new List<string>();
			PreviousResponders = new List<IPAddress>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new System.IO.MemoryStream());
			WriteIPList(innerwriter, PreviousResponders);

			if (GetAllAuthorities)
				innerwriter.Write((short)0xfff);
			else
				innerwriter.Write(NamingAuthority);
			innerwriter.Write(Scopes);

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			var tmp = reader.ReadInt16();
			if (tmp == 0xfff)
				GetAllAuthorities = true;
			else
				NamingAuthority = reader.ReadString(tmp);

			Scopes.AddRange(reader.ReadList());
		}
	}
}
