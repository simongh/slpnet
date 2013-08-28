using System;

namespace Discovery.Slp
{
	public enum ServiceHostNameType
	{
		Unknown,
		IP,
		IPX,
		Appletalk
	}

	public enum ServiceErrorCode
	{
		NoError = 0,
		LanguageNotSupported = 1,
		ParseError = 2,
		InvalidRegistration = 3,
		ScopeNotSupported = 4,
		AuthenticationUnknown = 5,
		AuthenticationAbsent = 6,
		AuthenticationFailed = 7,
		VersionNotSupported = 9,
		InternalError = 10,
		DABusyNow = 11,
		OptionNotUnderstood = 12,
		InvalidUpdate = 13,
		MessageNotSupported = 14,
		RefreshRejected = 15
	}

	internal static class Constants
	{
		public static readonly DateTime EPOCH = new DateTime(1970, 1, 1);
		public const string DEFAULTSCOPE = "DEFAULT";

		public const string VALIDTEXT = "\\([0-9a-f]{2})";
		public const string ATTRIBUTEVALUERESERVED = @"[\(\),\\!<=>~\x00-\x1F\x7F_*]";
		public const string ATTRIBUTETAGRESERVED = @"[\(\),\\!<=>~\x00-\x1F\x7F]";
		public const string SCOPERESERVED = @"[\(\),\\!<=>~\x00-\x1f\x7F]";
	}
}
