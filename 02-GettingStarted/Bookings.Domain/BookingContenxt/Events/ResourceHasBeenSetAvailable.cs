using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class ResourceHasBeenSetAvailable : IMessage
    {
        public Guid Id { get; set; }

        public ResourceHasBeenSetAvailable(Guid id)
        {
            Id = id;
        }
    }
}