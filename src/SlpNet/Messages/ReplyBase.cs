
namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Abstract base class for reply type messages
	/// </summary>
	internal abstract class ReplyBase : MessageBase
	{
		/// <summary>
		/// Gets the error code in this reply. Will be zero if there is no error
		/// </summary>
		public ServiceErrorCode ErrorCode
		{
			get;
			set;
		}
	}
}
