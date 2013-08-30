using System.Collections.Generic;
using System.IO;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Allows a UA to discover the attributes of a given service
	/// </summary>
	internal class AttributeReply : ReplyBase
	{
		protected override int FunctionId
		{
			get { return 7; }
		}

		/// <summary>
		/// Gets the attribute collection
		/// </summary>
		public AttributeCollection Attributes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the authentication blocks attached to this message
		/// </summary>
		public List<Security.AuthenticationBlock> AuthBlocks
		{
			get;
			private set;
		}

		public AttributeReply()
			: base()
		{
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new MemoryStream());
			innerwriter.Write((short)ErrorCode);
			Attributes.ToBytes(innerwriter);
			WriteAuthBlocks(innerwriter, AuthBlocks);

			writer.Write(innerwriter.Length, 3);

			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			ErrorCode = (ServiceErrorCode)reader.ReadInt16();
			Attributes = Services.Locator.GetInstance<AttributeCollection>(reader);
			ReadAuthBlocks(reader, AuthBlocks);
		}
	}
}
