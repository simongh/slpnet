using System;
using System.Configuration.Provider;

namespace Discovery.Slp.Configuration
{
	public abstract class StorageProviderBase : ProviderBase
	{
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			if (config == null)
				throw new ArgumentNullException("config");

			if (string.IsNullOrEmpty(name))
				name = "StorageProviderBase";

			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "Storage provider for the SLP DA");
			}

			base.Initialize(name, config);
		}
	}
}
