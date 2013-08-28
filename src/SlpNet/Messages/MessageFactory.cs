
namespace Discovery.Slp.Messages
{
	internal class MessageFactory
	{
		public static MessageFactory Instance
		{
			get;
			private set;
		}

		static MessageFactory()
		{
			Instance = new MessageFactory();
		}

		public MessageBase Read(SlpReader reader)
		{
			var v = reader.ReadByte();
			if (v != 1 && v != 2)
				throw new ServiceProtocolException(ServiceErrorCode.VersionNotSupported);

			var tmp = reader.ReadByte();
			MessageBase result = null;
			switch (tmp)
			{
				case 6:
					result = new AttributeRequest();
					break;
				case 7:
					result = new AttributeReply();
					break;
				default:
					throw new ServiceProtocolException(ServiceErrorCode.ParseError);
			}

			result.Version = v;
			result.Create(reader);

			return result;
		}
	}
}
