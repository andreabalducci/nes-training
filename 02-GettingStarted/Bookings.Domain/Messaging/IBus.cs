namespace Bookings.Domain.Messaging
{
	public interface IBus
	{
		void Send(IMessage message);
		void Publish(IMessage message);
	}
}