using System;

namespace Discovery.Slp
{
	public class ServiceException : Exception
	{
		public ServiceException()
			: base()
		{ }

		public ServiceException(string message)
			: base(message)
		{ }

		public ServiceException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ServiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{ }
	}
}
