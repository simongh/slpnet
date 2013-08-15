using System.Configuration;

namespace Discovery.Slp.Configuration
{
	public class SlpSection : ConfigurationSection
	{
		private const string SECTION = "slp";
		private const string PROTOCOL = "protocol";
		private const string USERAGENT = "userAgent";
		private const string SERVICEAGENT = "serviceAgent";
		private const string DIRECTORYAGENT = "directoryAgent";

		[ConfigurationProperty(PROTOCOL)]
		public Protocol Protocol
		{
			get { return (Protocol)this[PROTOCOL]; }
		}

		[ConfigurationProperty(USERAGENT)]
		public UserAgent UserAgent
		{
			get { return (UserAgent)this[USERAGENT]; }
		}

		[ConfigurationProperty(SERVICEAGENT)]
		public ServiceAgent ServiceAgent
		{
			get { return (ServiceAgent)this[SERVICEAGENT]; }
		}

		[ConfigurationProperty(DIRECTORYAGENT)]
		public DirectoryAgent DirectoryAgent
		{
			get { return (DirectoryAgent)this[DIRECTORYAGENT]; }
		}

		public static SlpSection Instance
		{
			get { return (SlpSection)ConfigurationManager.GetSection(SECTION); }
		}
	}
}
