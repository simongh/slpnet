using System;

namespace Discovery.Slp.Services
{
	/// <summary>
	/// Locates and creates instances of types.
	/// </summary>
	public static class Locator
	{
		private static IServiceProvider _provider;
		private static object _lock;

		/// <summary>
		/// Gets whether the locator has been set
		/// </summary>
		public static bool IsSet
		{
			get { return _provider != null; }
		}

		static Locator()
		{
			_lock = new object();
		}

		/// <summary>
		/// Sets the locator to use a given provider. The default provider will be used if none is specifed
		/// </summary>
		/// <param name="provider"></param>
		public static void SetProvider(IServiceProvider provider)
		{
			if (IsSet)
				throw new ApplicationException("The provider has already been set");

			_provider = provider;
		}

		/// <summary>
		/// Get an instance
		/// </summary>
		/// <typeparam name="T">Type to locate</typeparam>
		/// <param name="arguments">optional arguments to pass to the type</param>
		/// <returns>an instance of T</returns>
		public static T GetInstance<T>(params object[] arguments)
		{
			if (_provider == null)
				lock (_lock)
				{
					if (_provider == null)
						_provider = new DefaultProvider();
				}

			return _provider.GetInstance<T>(arguments);
		}

		public static object GetInstance(Type type, params object[] arguments)
		{
			if (_provider == null)
				lock (_lock)
				{
					if (_provider == null)
						_provider = new DefaultProvider();
				}

			return _provider.GetInstance(type, arguments);
		}
	}
}
