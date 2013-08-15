using System;
using System.Collections.Generic;

namespace Discovery.Slp
{
	/// <summary>
	/// Represents a service registration
	/// </summary>
	public class ServiceEntry
	{
		private TimeSpan _Lifetime;

		/// <summary>
		/// Occurs when an authentication block is encountered during parsing
		/// </summary>
		public static event EventHandler<Security.AuthenticatedEventArgs> Authenticated;

		/// <summary>
		/// Gets or sets the validity of this registration
		/// </summary>
		public TimeSpan Lifetime
		{
			get { return _Lifetime; }
			set
			{
				if (value.TotalSeconds > 0xffff)
					throw new ServiceException("The specified lifetime is greater than that allowed.");
				_Lifetime = value;
			}
		}

		/// <summary>
		/// Gets or sets the Uri of the service
		/// </summary>
		public ServiceUri Uri
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the list of authentication blocks
		/// </summary>
		public List<Security.AuthenticationBlock> AuthBlocks
		{
			get;
			private set;
		}

		public ServiceEntry()
		{
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		internal byte[] ToBytes()
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			ToBytes(ms);
			return ms.ToArray();
		}

		internal void ToBytes(System.IO.Stream stream)
		{
			if (Uri == null)
				throw new ServiceException("Service URL cannot be null.");

			stream.WriteByte(0);
			Utilities.WriteInt(stream, (int)Lifetime.TotalSeconds, 2);
			Utilities.WriteString(stream, Uri.ToString());
			Utilities.WriteInt(stream, AuthBlocks.Count, 1);

			foreach (var item in AuthBlocks)
			{
				item.ToByteArray(stream);
			}
		}

		internal static ServiceEntry FromBytes(System.IO.Stream stream)
		{
			if (stream.ReadByte() != 0)
				throw new ServiceProtocolException(ServiceErrorCode.ParseError);

			ServiceEntry result = new ServiceEntry();
			result.Lifetime = new TimeSpan(0, 0, Utilities.ReadInt(stream, 2));
			result.Uri = new ServiceUri(Utilities.ReadString(stream));

			int c = Utilities.ReadInt(stream, 1);
			for (int i = 0; i < c; i++)
			{
				var a = Security.AuthenticationBlock.Parse(stream);
				result.AuthBlocks.Add(a);
				OnAuthenticated(result, a);
			}

			return result;
		}

		private static void OnAuthenticated(ServiceEntry serviceEntry, Security.AuthenticationBlock authBlock)
		{
			if (Authenticated != null)
				Authenticated(null, new Security.AuthenticatedEventArgs(serviceEntry, authBlock));
		}
	}
}
