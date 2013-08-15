using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Discovery.Slp
{
	public class ServiceUri : IComparable<ServiceUri>, IEquatable<ServiceUri>
	{
		//private Regex _ServiceRe;
		private static Regex _TypeRe;
		private static Regex _SiteRe;
		//private Regex _UrlRe;
		//private Regex _AttrRe;
		private string _Output;
		
		/// <summary>
		/// Gets the original string passed to the constructor
		/// </summary>
		public string OriginalString
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service type name
		/// </summary>
		public string ServiceType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service type naming authority
		/// </summary>
		public string ServiceTypeAuthority
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the full service type
		/// </summary>
		public string FullServiceType
		{
			get { return ServiceType + (string.IsNullOrEmpty(ServiceTypeAuthority) ? "" : "." + ServiceTypeAuthority); }
		}

		/// <summary>
		/// Gets whether this uri contains a service type name
		/// </summary>
		public bool IsAbstract
		{
			get { return ServiceType == null; }
		}

		/// <summary>
		/// Gets the abstract service type for this service
		/// </summary>
		public ServiceUri AbstractType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the scheme or protocol
		/// </summary>
		public string Scheme
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the scheme or protocol naming authority
		/// </summary>
		public string SchemeAuthority
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the username. Username can only be specified in IP addresses
		/// </summary>
		public string Username
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the hostname
		/// </summary>
		public string Host
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the port when the address type is IP. Returns 0 when no port is specified
		/// </summary>
		public int Port
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the host name type
		/// </summary>
		public ServiceHostNameType HostNameType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets whether the uri has a path part
		/// </summary>
		public bool HasPath
		{
			get { return Path == null || Path.Length > 1; }
		}

		/// <summary>
		/// Gets the full path part
		/// </summary>
		public string Path
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the attributes collection
		/// </summary>
		public AttributeCollection Attributes
		{
			get;
			private set;
		}

		static ServiceUri()
		{
			_TypeRe = new Regex(@"(?::(?<typename>[a-z][a-z0-9+-]+)(?:\.(?<typenamingauth>[a-z][a-z0-9+-]+))?)?:(?<scheme>[a-z][a-z0-9+-]+)(?:\.(?<schemenamingauth>[a-z][a-z0-9+-]+))?:", RegexOptions.IgnoreCase);
			_SiteRe = new Regex(@"(?://(?:(?<user>(?:[a-z0-9$\-_\.~!\*'\(\),+;&=]|\\[0-9a-f])+)@)?(?<host>(?:[a-z0-9\-]+\.)*[a-z0-9\-]+)(?::(?<port>\d+))?|/ipx/(?<ipx>[0-9a-f]{8}:[0-9a-f]{12}:[0-9a-f]{4})|/at/(?<atsite>(?:[a-z0-9$-_.~]|\\[a-f0-9]){1,31}:(?:[a-z0-9$-_.~]|\\[a-f0-9]){1,31}(?:[a-z0-9$-_.~]|\\[a-f0-9]){1,31}))", RegexOptions.IgnoreCase);
		}

		private ServiceUri()
		{
//			_ServiceRe = new Regex(@"^service:.+:/.+$", RegexOptions.IgnoreCase);
//			_UrlRe = new Regex(@"(?:/([^;/]+))", RegexOptions.IgnoreCase);
//			_AttrRe = new Regex(@";([^;=]+)(?:=([^;=]+))?", RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// Initialises a new service URI
		/// </summary>
		/// <param name="uri">URI service string</param>
		public ServiceUri(string uri)
			: this()
		{
			OriginalString = uri;
			ToString();
		}

		public ServiceUri(string uri, AttributeCollection attributes)
			: this(uri)
		{
			Attributes = attributes;
		}

		/// <summary>
		/// Initialises a new service URI using an abstract type
		/// </summary>
		/// <param name="abstractUri">Service URI to use for the abstract type</param>
		/// <param name="uri">Service URI string</param>
		public ServiceUri(ServiceUri abstractUri, string uri)
			: this(uri)
		{
			if (!abstractUri.IsAbstract)
				throw new ServiceException("The abstract uri parameter was not an abstract service uri.");

			AbstractType = abstractUri;

			foreach (KeyValuePair<string, string> item in Attributes)
			{
				if (AbstractType.Attributes.ContainsKey(item.Key))
					throw new ServiceException("An attribute cannot be redeclared when an abstract type is defined.");
			}
		}

		/// <summary>
		/// Checks the validity of the hostname.
		/// </summary>
		/// <param name="name">Host name string to validate.</param>
		/// <returns>A ServiceHostNameType that inidicates the host name type, otherwise Unknown is returned</returns>
		public static ServiceHostNameType CheckHostName(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			Match m = _SiteRe.Match(name);
			if (m.Groups["host"] != null)
				return ServiceHostNameType.IP;
			if (m.Groups["ipx"] != null)
				return ServiceHostNameType.IPX;
			if (m.Groups["atsite"] != null)
				return ServiceHostNameType.Appletalk;

			return ServiceHostNameType.Unknown;
		}

		/// <summary>
		/// Determines whether the scheme is valid for a service URI.
		/// </summary>
		/// <param name="schemeName">A scheme name string optionally including a naming authority</param>
		/// <returns>returns true if the </returns>
		public static bool CheckSchemeName(string schemeName)
		{
			return _TypeRe.IsMatch(schemeName);
		}

		public int CompareTo(ServiceUri other)
		{
			throw new NotImplementedException();
		}

		public bool Equals(ServiceUri other)
		{
			return string.Compare(ToAbstractString(), other.ToAbstractString(), true) == 0;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ServiceUri);
		}

		public bool IsBaseOf(ServiceUri uri)
		{
			string tmp = AbstractType == null ? ServiceType : AbstractType.ServiceType;
			if (string.Compare(tmp, uri.ServiceType, true) != 0)
				return false;

			tmp = AbstractType == null ? ServiceTypeAuthority : AbstractType.ServiceTypeAuthority;
			if (string.Compare(tmp, ServiceTypeAuthority, true) != 0)
				return false;

			return true;
		}

		private string ToString(bool includeAbstract)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("service:");
			if (includeAbstract && AbstractType != null)
			{
				sb.Append(AbstractType.ServiceType);
				if (!string.IsNullOrEmpty(AbstractType.ServiceTypeAuthority))
					sb.AppendFormat(".{0}", AbstractType.ServiceTypeAuthority);
				sb.Append(":");
			}
			else if (IsAbstract)
			{
				sb.Append(ServiceType);
				if (!string.IsNullOrEmpty(ServiceTypeAuthority))
					sb.AppendFormat(".{0}", ServiceTypeAuthority);
				sb.Append(":");
			}
			sb.Append(Scheme);
			if (!string.IsNullOrEmpty(SchemeAuthority))
				sb.AppendFormat(".{0}", SchemeAuthority);
			sb.Append(":");

			switch (HostNameType)
			{
				case ServiceHostNameType.Unknown:
					break;
				case ServiceHostNameType.IP:
					sb.Append("//");
					if (!string.IsNullOrEmpty(Username))
						sb.AppendFormat("{0}@", Username);
					sb.Append(Host);
					if (Port > 0)
						sb.AppendFormat(":{0}", Port);
					break;
				case ServiceHostNameType.IPX:
					sb.AppendFormat("/ipx/{0}", Host);
					break;
				case ServiceHostNameType.Appletalk:
					sb.AppendFormat("/at/{0}", Host);
					break;
			}

			if (HasPath)
				sb.Append(Path);

			foreach (KeyValuePair<string, string> item in Attributes)
			{
				sb.AppendFormat(";{0}", item.Key);
				if (!string.IsNullOrEmpty(item.Value))
					sb.AppendFormat("={0}", item.Value);
			}
			if (includeAbstract)
			{
				foreach (KeyValuePair<string,string> item in AbstractType.Attributes)
				{
				sb.AppendFormat(";{0}", item.Key);
				if (!string.IsNullOrEmpty(item.Value))
					sb.AppendFormat("={0}", item.Value);
				}
			}

			return sb.ToString();
		}

		public override string ToString()
		{
			if (_Output == null)
				_Output = ToString(false);
			return _Output;
		}

		/// <summary>
		/// Returns the full abstract definition of the service
		/// </summary>
		/// <returns>string</returns>
		public string ToAbstractString()
		{
			return ToString(true);
		}

		#region IComparable<ServiceUri> Members

		int IComparable<ServiceUri>.CompareTo(ServiceUri other)
		{
			return this.CompareTo(other);
		}

		#endregion

		#region IEquatable<ServiceUri> Members

		bool IEquatable<ServiceUri>.Equals(ServiceUri other)
		{
			return Equals(other);
		}

		#endregion

		public static bool operator ==(ServiceUri uri1, ServiceUri uri2)
		{
			return uri1.Equals(uri2);
		}

		public static bool operator !=(ServiceUri uri1, ServiceUri uri2)
		{
			return !uri1.Equals(uri2);
		}
	}
}
