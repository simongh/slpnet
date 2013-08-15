using System.Configuration;

namespace Discovery.Slp.Configuration
{
	public class DirectoryAgent : AgentBase
	{
		private const string DEFAULTPROVIDER = "defaultProvider";
		private const string PROVIDERS = "providers";

		[ConfigurationProperty(DEFAULTPROVIDER, IsRequired = true)]
		[StringValidator(MinLength = 1)]
		public string DefaultProvider
		{
			get { return (string)this[DEFAULTPROVIDER]; }
			set { this[DEFAULTPROVIDER] = value; }
		}

		[ConfigurationProperty(PROVIDERS)]
		public ProviderSettingsCollection Providers
		{
			get { return (ProviderSettingsCollection)this[PROVIDERS]; }
		}
	}
}
