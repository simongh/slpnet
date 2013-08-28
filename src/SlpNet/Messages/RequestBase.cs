using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Discovery.Slp.Messages
{
	internal abstract class RequestBase : MessageBase
	{
		protected void WriteIPList(SlpWriter writer, IEnumerable<IPAddress> list)
		{
			StringBuilder sb = new StringBuilder();
			foreach (System.Net.IPAddress item in list)
			{
				if (sb.Length > 0)
					sb.Append(",");

				if (item.AddressFamily != AddressFamily.InterNetwork)
					throw new ServiceException("IP v4 addresses are the only allowable type.");

				sb.Append(item.ToString());
			}

			writer.WriteRaw(sb.ToString());
		}

		protected void ReadIPList(SlpReader reader, IList<IPAddress> list)
		{
			foreach (var item in reader.ReadList())
			{
				var i = IPAddress.Parse(item);
				if (i.AddressFamily != AddressFamily.InterNetwork)
					throw new ServiceException("IP v4 addresses are the only allowable type.");
				list.Add(i);
			}
		}
	}
}
