
namespace Discovery.Slp.Messages
{
	/// <summary>
	/// Sent by a DA in response to a service registration
	/// </summary>
	internal class ServiceAcknowledgement : ReplyBase
	{
		protected override int FunctionId
		{
			get { return 5; }
		}

		public override void ToBytes(SlpWriter writer)
		{
			base.ToBytes(writer);

			WriteHeader(writer);
		}
	}
}
