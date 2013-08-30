using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Contains a list of service entries for a UA
	/// </summary>
	internal class ServiceReply : ReplyBase
	{
		protected override int FunctionId
		{
			get { return 2; }
		}

		/// <summary>
		/// Gets the service entries available
		/// </summary>
		public ReadOnlyCollection<ServiceEntry> Services
		{
			get;
			private set;
		}

		public ServiceReply(IList<ServiceEntry> list)
			: this()
		{
			Services = new ReadOnlyCollection<ServiceEntry>(list);
		}

		public ServiceReply()
			: base()
		{ }

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			var innerwriter = new SlpWriter(new MemoryStream());
			innerwriter.Write((short)ErrorCode);
			innerwriter.Write((short)Services.Count);
			foreach (var item in Services)
			{
				item.ToBytes(innerwriter);
			}

			writer.Write(innerwriter.Length, 3);
			WriteHeader(writer);
			writer.Write(innerwriter);
		}

		internal override void Create(SlpReader reader)
		{
			base.Create(reader);

			var tmp = reader.ReadInt16();
			var result = new ServiceEntry[tmp];
			for (int i = 0; i < tmp; i++)
			{
				result[i] = Slp.Services.Locator.GetInstance<ServiceEntry>(reader);
			}

			Services = new ReadOnlyCollection<ServiceEntry>(result);
		}
	}
}
