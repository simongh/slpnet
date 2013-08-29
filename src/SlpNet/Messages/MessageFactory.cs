
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
				case 1:
					result = new ServiceRequest();
					break;
				case 2:
					result = new ServiceReply();
					break;
				case 3:
					result = new ServiceRegistrationRequest();
					break;
				case 4:
					result = new ServiceDeregistrationRequest();
					break;
				case 5:
					result = new ServiceAcknowledgement();
					break;
				case 6:
					result = new AttributeRequest();
					break;
				case 7:
					result = new AttributeReply();
					break;
				case 8:
					result = new DirectoryAgentAdvert();
					break;
				case 9:
					result = new ServiceTypeRequest();
					break;
				case 10:
					result = new ServiceTypeReply();
					break;
				case 11:
					result = new ServiceAgentAdvert();
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
