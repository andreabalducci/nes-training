using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt
{
    public class RisorsaResaPrenotabile : IMessage
    {
        public Guid Id { get; set; }

        public RisorsaResaPrenotabile(Guid id)
        {
            Id = id;
        }
    }
}