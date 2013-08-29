
namespace Discovery.Slp
{
	/// <summary>
	/// Singleton class to return new message ID's
	/// </summary>
	internal class MessageCounter
	{
		private int _Counter;

		/// <summary>
		/// Gets the counter instance
		/// </summary>
		public static MessageCounter Instance
		{
			get;
			private set;
		}

		static MessageCounter()
		{
			Instance = new MessageCounter();
		}

		private MessageCounter()
		{ }

		/// <summary>
		/// Retrieves the next message ID
		/// </summary>
		/// <returns>new int message ID</returns>
		public int GetNextId()
		{
			lock (this)
			{
				_Counter++;
				return _Counter;
			}
		}
	}
}
