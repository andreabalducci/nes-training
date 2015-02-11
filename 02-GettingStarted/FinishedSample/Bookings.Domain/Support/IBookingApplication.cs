using Bookings.Domain.Messaging;
using CommonDomain.Persistence;

namespace Bookings.Domain.Support
{
    public interface IBookingApplication
    {
        void Stop();
        void Accept(IMessage message);
        bool HasPendingMessages { get; }
        IRepository CreateRepository();
    }
}