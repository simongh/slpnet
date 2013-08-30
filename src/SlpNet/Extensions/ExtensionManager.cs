using System;
using System.Collections.Generic;

namespace Discovery.Slp.Extensions
{
	/// <summary>
	/// Manages registered extensions
	/// </summary>
	public class ExtensionManager
	{
		private Dictionary<int, Type> _RegisteredExtensions;

		public ExtensionManager()
		{
			_RegisteredExtensions = new Dictionary<int, Type>();
		}

		/// <summary>
		/// Registers a new extension ID
		/// </summary>
		/// <param name="extension">An instance of the extension</param>
		public void RegisterExtension<T>(int id) where T : ExtensionBase
		{
			if (_RegisteredExtensions.ContainsKey(id))
				throw new ServiceException("An extension with that ID is already registered.");

			_RegisteredExtensions.Add(id, typeof(T));
		}

		/// <summary>
		/// Parses the binary data and returns an extension
		/// </summary>
		/// <param name="data">Data to parse. The data may contain multiple extensions, but only the first one is returned</param>
		/// <returns>New extension if the extension is supported</returns>
		internal ExtensionBase GetExtension(SlpReader reader)
		{
			var id = reader.ReadInt16();
			if (id < 0 || id > 0x8ff)
				throw new ServiceProtocolException(ServiceErrorCode.OptionNotUnderstood, "The ID was out of the allowed range of possible values.");

			if (id >= 0x400 && id < 0x800)
			{
				if (!_RegisteredExtensions.ContainsKey(id))
					throw new ServiceProtocolException(ServiceErrorCode.OptionNotUnderstood);
			}

			if (!_RegisteredExtensions.ContainsKey(id))
				return null;

			var result = (ExtensionBase)Services.Locator.GetInstance(_RegisteredExtensions[id]);
			return result.Create(reader);
		}

	}
}
