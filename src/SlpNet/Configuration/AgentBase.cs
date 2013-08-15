using System.Configuration;

namespace Discovery.Slp.Configuration
{
	/// <summary>
	/// Base class for agent config
	/// </summary>
	public abstract class AgentBase : ConfigurationElement
	{
		private const string BROADCASTONLY = "broadcastOnly";
		private const string BROADCOSTADDRESS = "broadcastAddress";
		private const string MULTICASTTTL = "multicastTTL";
		private const string SCOPES = "scopes";
		private const string PORT = "port";
		private const string MCASTADDRESS = "multicastAddress";
		private const string USETCP = "useTcp";

		/// <summary>
		/// Gets or sets whether to use broadcast instead of multicast. Default value is false
		/// </summary>
		[ConfigurationProperty(BROADCASTONLY, DefaultValue = false)]
		public bool UseBroadcastOnly
		{
			get { return (bool)this[BROADCASTONLY]; }
			set { this[BROADCASTONLY] = value; }
		}

		/// <summary>
		/// Gets or sets the broadcast address
		/// </summary>
		[ConfigurationProperty(BROADCOSTADDRESS)]
		[RegexStringValidator(@"(\d{1,3}\.){3}\d{1,3}")]
		public string BroadcastAddress
		{
			get { return (string)this[BROADCOSTADDRESS]; }
			set { this[BROADCOSTADDRESS] = value; }
		}

		/// <summary>
		/// Gets or sets the multicast TTL value. Default value is 255.
		/// </summary>
		[ConfigurationProperty(MULTICASTTTL, DefaultValue = 255)]
		[IntegerValidator(MinValue = 0)]
		public int MulticastTtl
		{
			get { return (int)this[MULTICASTTTL]; }
			set { this[MULTICASTTTL] = value; }
		}

		/// <summary>
		/// Gets or sets the list of scopes used by the agent.
		/// </summary>
		[ConfigurationProperty(SCOPES)]
		public string[] Scopes
		{
			get { return (string[])this[SCOPES]; }
			set { this[SCOPES] = value; }
		}

		/// <summary>
		/// Gets or sets the port. Default value is 427
		/// </summary>
		[ConfigurationProperty(PORT, DefaultValue = 427)]
		[IntegerValidator(MinValue = 0, MaxValue = 0xffff)]
		public int Port
		{
			get { return (int)this[PORT]; }
			set { this[PORT] = value; }
		}

		/// <summary>
		/// Gets or sets the multicast IP address. Default value is 239.255.255.253
		/// </summary>
		[ConfigurationProperty(MCASTADDRESS, DefaultValue = "239.255.255.253")]
		[RegexStringValidator(@"(\d{1,3}\.){3}\d{1,3}")]
		public string MulticastAddress
		{
			get { return (string)this[MCASTADDRESS]; }
			set { this[MCASTADDRESS] = value; }
		}

		/// <summary>
		/// Gets or sets whether TCP connections can be used
		/// </summary>
		[ConfigurationProperty(USETCP, DefaultValue = true)]
		public bool UseTcp
		{
			get { return (bool)this[USETCP]; }
			set { this[USETCP] = value; }
		}
	}
}
