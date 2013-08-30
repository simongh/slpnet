using System;

namespace Discovery.Slp.Services
{
	/// <summary>
	/// Default implementation for service location
	/// </summary>
	internal class DefaultProvider : IServiceProvider
	{
		public T GetInstance<T>(params object[] arguments)
		{
			return (T)GetInstance(typeof(T), arguments);
		}

		public object GetInstance(Type type, params object[] arguments)
		{
			var result = GetStaticInstance(type);
			if (result != null)
				return result;

			result = UseFactory(type, arguments);
			if (result != null)
				return result;

			type = MatchInterface(type);
			return Activator.CreateInstance(type, arguments);
		}

		private object GetStaticInstance(Type type)
		{
			if (type == typeof(Messages.MessageFactory))
				return Messages.MessageFactory.Instance;

			if (type == typeof(Configuration.SlpSection))
				return Configuration.SlpSection.Instance;

			if (type == typeof(MessageCounter))
				return MessageCounter.Instance;

			return null;
		}

		private object UseFactory(Type type, params object[] arguments)
		{
			if (type == typeof(AttributeCollection))
				return EntityFactory.Instance.CreateAttributeCollection((SlpReader)arguments[0]);
			if (type == typeof(ServiceEntry))
				return EntityFactory.Instance.CreateServiceEntry((SlpReader)arguments[0]);
			if (type == typeof(Security.AuthenticationBlock))
				return EntityFactory.Instance.CreateAuthenticationBlock((SlpReader)arguments[0]);

			return null;
		}

		private Type MatchInterface(Type type)
		{
			if (!type.IsInterface)
				return type;

			throw new ApplicationException("Unsupported interface requested");
		}
	}
}
