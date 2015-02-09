using Bookings.Domain.Support;

namespace Bookings.Domain.Messaging
{
	public interface IMessageHandler
	{
	}

	public interface IMessageHandler<T> : IMessageHandler where T : class, IMessage
	{
		void Handle(T message);
	}
}