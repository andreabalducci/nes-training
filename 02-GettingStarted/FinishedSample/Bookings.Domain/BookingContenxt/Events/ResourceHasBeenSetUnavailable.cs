using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class ResourceHasBeenSetUnavailable: IMessage
    {
        public Guid Id { get; set; }

        public ResourceHasBeenSetUnavailable(Guid id)
        {
            Id = id;
        }
    }
}