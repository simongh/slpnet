
namespace Discovery.Slp
{
	internal class EntityFactory
	{
		public static EntityFactory Instance
		{
			get;
			private set;
		}

		static EntityFactory()
		{
			Instance = new EntityFactory();
		}

		private EntityFactory()
		{ }

		public virtual ServiceEntry CreateServiceEntry(SlpReader reader)
		{
			if (reader.ReadByte() != 0)
				throw new ServiceProtocolException(ServiceErrorCode.ParseError);

			var result = new ServiceEntry();
			result.Lifetime = reader.ReadTimeSpan();
			result.Uri = new ServiceUri(reader.ReadString());

			var count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				var a = CreateAuthenticationBlock(reader);
				result.AuthBlocks.Add(a);
				ServiceEntry.OnAuthenticated(result, a);
			}

			return result;
		}

		public virtual Security.AuthenticationBlock CreateAuthenticationBlock(SlpReader reader)
		{
			var result = new Security.AuthenticationBlock();
			result.Descriptor = reader.ReadInt16();
			var length = reader.ReadInt16();
			result.TimeStamp = reader.ReadDateTime();
			result.SpiString = reader.ReadString();

			result.Data = reader.ReadBytes(length);

			return result;
		}

		public virtual AttributeCollection CreateAttributeCollection(SlpReader reader)
		{
			var tmp = reader.ReadRawString();

			var result = new AttributeCollection();
			foreach (var item in tmp.Split(','))
			{
				var pair = item.Split('=');
				string key = null, value = null;
				if (pair.Length == 1)
					key = reader.Unescape(pair[0]);
				else if (pair.Length == 2)
				{
					key = reader.Unescape(pair[0]);
					value = reader.Unescape(pair[1]);
				}
				else
					throw new ServiceProtocolException(ServiceErrorCode.ParseError);

				result.Add(key, value);
			}

			return result;
		}
	}
}
