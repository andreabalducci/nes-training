using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
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