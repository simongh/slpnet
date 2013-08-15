using System.Configuration;

namespace Discovery.Slp.Configuration
{
	public class ServiceAgent : AgentBase
	{
		private const string DAADDRESS = "dAAddress";
		private const string NODADISCOVERY = "noDiscovery";

		[ConfigurationProperty(DAADDRESS)]
		public System.Net.IPAddress DAAddress
		{
			get { return (System.Net.IPAddress)this[DAADDRESS]; }
			set { this[DAADDRESS] = value; }
		}

		[ConfigurationProperty(NODADISCOVERY)]
		public bool NoDADiscovery
		{
			get { return (bool)this[NODADISCOVERY]; }
			set { this[NODADISCOVERY] = value; }
		}
	}
}
