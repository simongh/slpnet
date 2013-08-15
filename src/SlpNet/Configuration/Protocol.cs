using System;
using System.Configuration;

namespace Discovery.Slp.Configuration
{
	/// <summary>
	/// Defines protocol timing values for the SLP client
	/// </summary>
	public class Protocol : ConfigurationElement
	{
		private const string MULTICASTMAXWAIT = "multicastMaxWait";
		private const string STARTDADELAY = "startDADelay";
		private const string RETRYINTERVAL = "retryInterval";
		private const string MAXRETRY = "maxRetryDelay";
		private const string DAHEARTBEAT = "dAHeartbeatInterval";
		private const string DAFINDINTERVAL = "minDAFindInterval";
		private const string REGDELAYPASSIVE = "passiveRegisterDelay";
		private const string REGDELAYACTIVE = "activeRegisterDelay";
		private const string IDLETIMEOUT = "idleTimeout";

		/// <summary>
		/// Gets or sets the max seconds to wait for multicast response
		/// </summary>
		[ConfigurationProperty(MULTICASTMAXWAIT, DefaultValue = 15)]
		[IntegerValidator(MinValue = 0)]
		public int MaxMulticastWait
		{
			get { return (int)this[MULTICASTMAXWAIT]; }
			set { this[MULTICASTMAXWAIT] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait before starting DA discovery
		/// </summary>
		[ConfigurationProperty(STARTDADELAY, DefaultValue = 3)]
		[IntegerValidator(MinValue = 0)]
		public int DADiscoveryDelay
		{
			get { return (int)this[STARTDADELAY]; }
			set { this[STARTDADELAY] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait before retrying a request
		/// </summary>
		[ConfigurationProperty(RETRYINTERVAL, DefaultValue = 2)]
		[IntegerValidator(MinValue = 0)]
		public int RetryInterval
		{
			get { return (int)this[RETRYINTERVAL]; }
			set { this[RETRYINTERVAL] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait for unicast retry
		/// </summary>
		[ConfigurationProperty(MAXRETRY, DefaultValue = 15)]
		[IntegerValidator(MinValue = 0)]
		public int MaxRetryInterval
		{
			get { return (int)this[MAXRETRY]; }
			set { this[MAXRETRY] = value; }
		}

		/// <summary>
		/// Gets or sets the interval between DA adverts
		/// </summary>
		[ConfigurationProperty(DAHEARTBEAT, DefaultValue = "3:0:0")]
		[PositiveTimeSpanValidator()]
		public TimeSpan DAHeartbeatInterval
		{
			get { return (TimeSpan)this[DAHEARTBEAT]; }
			set { this[DAHEARTBEAT] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait before repeating DA discovery
		/// </summary>
		[ConfigurationProperty(DAFINDINTERVAL, DefaultValue = "0:15:0")]
		[PositiveTimeSpanValidator]
		public TimeSpan DAFindInterval
		{
			get { return (TimeSpan)this[DAFINDINTERVAL]; }
			set { this[DAFINDINTERVAL] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait before registering services with passive DA discovery
		/// </summary>
		[ConfigurationProperty(REGDELAYPASSIVE, DefaultValue = 2)]
		[IntegerValidator(MinValue = 0)]
		public int PassiveServiceRegisterDelay
		{
			get { return (int)this[REGDELAYPASSIVE]; }
			set { this[REGDELAYPASSIVE] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds to wait before registering services with active DA discovery
		/// </summary>
		[ConfigurationProperty(REGDELAYACTIVE, DefaultValue = 2)]
		[IntegerValidator(MinValue = 0)]
		public int ActiveServiceRegisterDelay
		{
			get { return (int)this[REGDELAYACTIVE]; }
			set { this[REGDELAYACTIVE] = value; }
		}

		/// <summary>
		/// Gets or sets the seconds before a DA or SA closes idle connections
		/// </summary>
		[ConfigurationProperty(IDLETIMEOUT, DefaultValue = 300)]
		[IntegerValidator(MinValue = 0)]
		public int IdleTimeout
		{
			get { return (int)this[IDLETIMEOUT]; }
			set { this[IDLETIMEOUT] = value; }
		}
	}
}
