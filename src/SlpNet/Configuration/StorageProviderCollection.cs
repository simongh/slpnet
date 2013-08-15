using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace Discovery.Slp.Configuration
{
	public class StorageProviderCollection : ProviderCollection, IEnumerable<StorageProviderBase>
	{
		public new StorageProviderBase this[string name]
		{
			get { return (StorageProviderBase)this[name]; }
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (!(provider is StorageProviderBase))
				throw new ArgumentException("Provider is not of the correct type", "provider");

			base.Add(provider);
		}

		public new IEnumerator<StorageProviderBase> GetEnumerator()
		{
			ICollection collection = this;

			foreach (object provider in collection)
			{
				yield return (StorageProviderBase)provider;
			}
		}
	}
}
