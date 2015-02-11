using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class RisorsaResaNonPrenotabile: IMessage
    {
        public Guid Id { get; set; }

        public RisorsaResaNonPrenotabile(Guid id)
        {
            Id = id;
        }
    }
}