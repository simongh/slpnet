using System;

namespace Discovery.Slp
{
	public class ServiceProtocolException : ServiceException
	{
		private const string c_msg = "A service protocol exception occurred with code {0}. {1}";

		public ServiceErrorCode ErrorCode
		{
			get;
			private set;
		}

		public ServiceProtocolException(ServiceErrorCode errorCode)
			: base(string.Format(c_msg, errorCode, null))
		{
			ErrorCode = errorCode;
		}

		public ServiceProtocolException(ServiceErrorCode errorCode, string message)
			: base(string.Format(c_msg, errorCode, message))
		{ }

		public ServiceProtocolException(ServiceErrorCode errorCode, Exception innerException)
			: base(string.Format(c_msg, errorCode, null), innerException)
		{
			ErrorCode = errorCode;
		}
	}
}
