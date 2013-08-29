using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Returns a list of service types supported by agents for a UA
	/// </summary>
	internal class ServiceTypeReply : ReplyBase
	{
		protected override int FunctionId
		{
			get { return 10; }
		}

		/// <summary>
		/// Gets a list of Service URI's containing a service type
		/// </summary>
		public ReadOnlyCollection<ServiceUri> ServiceTypes
		{
			get;
			private set;
		}

		public ServiceTypeReply(IList<ServiceUri> list)
			: this()
		{
			ServiceTypes = new ReadOnlyCollection<ServiceUri>(list);
		}

		public ServiceTypeReply()
			: base()
		{ }

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new MemoryStream());
			innerwriter.Write((short)ErrorCode);
			innerwriter.Write(ServiceTypes.Select(s => s.FullServiceType));

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			var tmp = reader.ReadList();
			ServiceTypes = new ReadOnlyCollection<ServiceUri>(tmp.Select(s => new ServiceUri("service:" + s)).ToArray());
		}
	}
}
