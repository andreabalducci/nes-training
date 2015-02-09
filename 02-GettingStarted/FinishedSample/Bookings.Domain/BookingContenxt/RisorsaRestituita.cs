using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt
{
    public class RisorsaRestituita : IMessage
    {
        public Guid Id { get; set; }

        public RisorsaRestituita(Guid id)
        {
            Id = id;
        }
    }
}