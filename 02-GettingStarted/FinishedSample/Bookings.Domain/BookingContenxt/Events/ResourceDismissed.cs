using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class ResourceDismissed : IMessage
    {
        public Guid Id { get; set; }

        public ResourceDismissed(Guid id)
        {
            Id = id;
        }
    }
}