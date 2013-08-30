using System;
using System.Collections.Generic;
using System.Linq;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Base class for all SLP messages
	/// </summary>
	internal abstract class MessageBase
	{
		private int _ExtensionOffset;

		/// <summary>
		/// Occurs when an authentication block is encountered during parsing
		/// </summary>
		public static event EventHandler<Security.AuthenticatedEventArgs> Authenticated;

		/// <summary>
		/// Gets or sets the protocol version of the message. Should be 2, although limited support for 1 is available
		/// </summary>
		public int Version
		{
			get;
			set;
		}

		/// <summary>
		/// Gets whether this message has been received over the network
		/// </summary>
		public bool IsMessageReceived
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets the function ID, identifying thr type of message
		/// </summary>
		protected abstract int FunctionId
		{
			get;
		}

		/// <summary>
		/// Gets or sets whether the length of this message exceeds that of a datagram
		/// </summary>
		public bool IsOverFlow
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether this is a new service request
		/// </summary>
		public bool IsFresh
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets whether this message uses multicast/brodcast
		/// </summary>
		public bool IsMulticast
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of extensions included with this message
		/// </summary>
		public IList<Extensions.ExtensionBase> Extensions
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the message ID
		/// </summary>
		public int MessageId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the language tag
		/// </summary>
		public string Language
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the sender of this message
		/// </summary>
		public System.Net.IPAddress Sender
		{
			get;
			set;
		}

		public MessageBase()
		{
			Version = 2;
		}

		protected static void OnAuthenticated(MessageBase message, Security.AuthenticationBlock authBlock)
		{
			if (Authenticated != null)
				Authenticated(message, new Security.AuthenticatedEventArgs(null, authBlock));
		}

		/// <summary>
		/// Convert this message to bytes
		/// </summary>
		/// <param name="writer">writer</param>
		public virtual void ToBytes(SlpWriter writer)
		{
			writer.Write((byte)Version);
			writer.Write((byte)FunctionId);
		}

		/// <summary>
		/// Add the message header bytes
		/// </summary>
		/// <param name="writer">writer</param>
		protected void WriteHeader(SlpWriter writer)
		{
			int flags = IsOverFlow ? 0x8 : 0x0;
			flags |= IsFresh ? 0x4 : 0x0;
			flags |= IsMulticast ? 0x2 : 0x0;
			writer.Write((byte)flags);

			writer.Write((int)0);

			writer.Write((short)MessageId);
			writer.Write(Language);
		}

		/// <summary>
		/// Read the message bytes and set the properties on this instance
		/// </summary>
		/// <param name="reader"></param>
		internal virtual void Create(SlpReader reader)
		{
			reader.ReadInt24();

			var flags = reader.ReadByte();
			IsOverFlow = (flags & 0x80) == 0x80;
			IsFresh = (flags & 0x40) == 0x40;
			IsMulticast = (flags & 0x20) == 0x20;

			reader.ReadByte();
			_ExtensionOffset = reader.ReadInt24();
			MessageId = reader.ReadInt16();
			Language = reader.ReadString();
		}

		/// <summary>
		/// Write the authblocks bytes
		/// </summary>
		/// <param name="writer">writer</param>
		/// <param name="authBlocks">list of authblocks</param>
		protected void WriteAuthBlocks(SlpWriter writer, IEnumerable<Security.AuthenticationBlock> authBlocks)
		{
			writer.Write((byte)authBlocks.Count());
			foreach (var item in authBlocks)
			{
				item.ToBytes(writer);
			}
		}

		/// <summary>
		/// Read the authblocks bytes and create any authblocks, firing the event when a block is added
		/// </summary>
		/// <param name="reader">reader</param>
		/// <param name="authBlocks">list of authblocks</param>
		protected void ReadAuthBlocks(SlpReader reader, ICollection<Security.AuthenticationBlock> authBlocks)
		{
			var count = reader.ReadByte();
			for (int i = 0; i < count; i++)
			{
				var a = Services.Locator.GetInstance<Security.AuthenticationBlock>(reader);
				authBlocks.Add(a);
				OnAuthenticated(this, a);
			}
		}
	}
}
