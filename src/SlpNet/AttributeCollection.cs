using System;
using System.Collections.Generic;
using System.Text;

namespace Discovery.Slp
{
	public class AttributeCollection : IDictionary<string, string>
	{
		private Dictionary<string, string> _InnerCollection;
		private bool _IsReadOnly;

		public AttributeCollection()
		{ }

		public AttributeCollection(Dictionary<string, string> innerCollection)
			: this()
		{
			_IsReadOnly = true;
			_InnerCollection = innerCollection;
		}

		public AttributeCollection(Dictionary<string, string> innerCollection, bool readOnly)
			: this(innerCollection)
		{
			_IsReadOnly = readOnly;
		}

		#region IDictionary<string,string> Members

		public void Add(string key, string value)
		{
			if (IsReadOnly)
				throw new NotSupportedException();

			_InnerCollection.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return _InnerCollection.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return _InnerCollection.Keys; }
		}

		public bool Remove(string key)
		{
			if (IsReadOnly)
				throw new NotSupportedException();

			return _InnerCollection.Remove(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return _InnerCollection.TryGetValue(key, out value);
		}

		public ICollection<string> Values
		{
			get { return _InnerCollection.Values; }
		}

		public string this[string key]
		{
			get { return _InnerCollection[key]; }
			set
			{
				if (IsReadOnly)
					throw new NotSupportedException();

				_InnerCollection[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,string>> Members

		public void Add(KeyValuePair<string, string> item)
		{
			this.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			if (IsReadOnly)
				throw new NotSupportedException();

			_InnerCollection.Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return ((ICollection<KeyValuePair<string, string>>)_InnerCollection).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, string>>)_InnerCollection).CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _InnerCollection.Count; }
		}

		public bool IsReadOnly
		{
			get { return _IsReadOnly; }
			internal set { _IsReadOnly = value; }
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			if (IsReadOnly)
				throw new NotSupportedException();

			return ((ICollection<KeyValuePair<string, string>>)_InnerCollection).Remove(item);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,string>> Members

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return _InnerCollection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _InnerCollection.GetEnumerator();
		}

		#endregion

		public static byte[] GetBytes(string value)
		{
			if (!value.StartsWith(@"\FF", StringComparison.InvariantCultureIgnoreCase))
				throw new ArgumentException("The value is not a valid byte string");

			if (value.Length % 3 != 0)
				throw new ArgumentException("The string length was incorrect");

			byte[] buffer = new byte[value.Length / 3];
			for (int i = 0; i < value.Length; i += 3)
			{
				buffer[i / 3] = byte.Parse("0x" + value.Substring(i + 1, 2));
			}

			return buffer;
		}

		public static string GetString(byte[] value)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\\FF");

			foreach (byte item in value)
			{
				sb.AppendFormat(@"\{0:X}", item);
			}

			return sb.ToString();
		}

		internal void ToBytes(System.IO.Stream stream)
		{
			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<string, string> item in _InnerCollection)
			{
				if (sb.Length > 0)
					sb.Append(",");

				sb.Append("(");
				sb.Append(Utilities.EscapeString(item.Key, Utilities.AttributeTagReserved));
				if (!string.IsNullOrEmpty(item.Value))
				{
					sb.Append("=");
					if (item.Value.StartsWith("\\FF", StringComparison.InvariantCultureIgnoreCase))
						sb.Append(item.Value);
					else
						sb.Append(Utilities.EscapeString(item.Value, Utilities.AttributeValueReserved));
				}
				sb.Append(")");
			}

			Utilities.WriteRawString(stream, sb.ToString());
		}

		internal static AttributeCollection FromBytes(System.IO.Stream stream)
		{
			string tmp = Utilities.ReadRawString(stream);

			AttributeCollection result = new AttributeCollection();
			foreach (string item in tmp.Split(','))
			{
				string[] pair = item.Split('=');
				if (pair.Length == 1)
					result.Add(Utilities.UnescapeString(pair[0]), null);
				else if (pair.Length == 2)
					result.Add(Utilities.UnescapeString(pair[0]), Utilities.UnescapeString(pair[1]));
				else
					throw new ServiceProtocolException(ServiceErrorCode.ParseError);
			}
			return result;
		}
	}
}
