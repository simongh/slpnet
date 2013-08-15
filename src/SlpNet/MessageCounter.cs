
namespace Discovery.Slp
{
	/// <summary>
	/// Singleton class to return new message ID's
	/// </summary>
	internal class MessageCounter
	{
		private static readonly MessageCounter _Instance;

		private int _Counter;

		/// <summary>
		/// Gets the counter instance
		/// </summary>
		public static MessageCounter Instance
		{
			get { return _Instance; }
		}

		static MessageCounter()
		{
			_Instance = new MessageCounter();
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
