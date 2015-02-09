using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt
{
    public class RisorsaPresa: IMessage
    {
        public Guid Id { get; set; }

        public RisorsaPresa(Guid id)
        {
            Id = id;
        }
    }
}