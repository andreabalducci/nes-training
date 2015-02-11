using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class ResourceReturned : IMessage
    {
        public Guid Id { get; set; }

        public ResourceReturned(Guid id)
        {
            Id = id;
        }
    }
}