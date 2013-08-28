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
		public IList<Security.AuthenticationBlock> AuthBlocks
		{
			get;
			private set;
		}

		public ServiceEntry()
		{
			AuthBlocks = new List<Security.AuthenticationBlock>();
		}

		internal void ToBytes(SlpWriter writer)
		{
			if (Uri == null)
				throw new ServiceException("Service URL cannot be null.");

			writer.Write((byte)0);
			writer.Write((short)Lifetime.TotalSeconds);
			writer.Write(Uri.ToString());
			writer.Write((byte)AuthBlocks.Count);

			foreach (var item in AuthBlocks)
			{
				item.ToBytes(writer);
			}
		}

		public static void OnAuthenticated(ServiceEntry serviceEntry, Security.AuthenticationBlock authBlock)
		{
			if (Authenticated != null)
				Authenticated(null, new Security.AuthenticatedEventArgs(serviceEntry, authBlock));
		}
	}
}
