using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class ResourceLent: IMessage
    {
        public Guid Id { get; set; }

        public ResourceLent(Guid id)
        {
            Id = id;
        }
    }
}