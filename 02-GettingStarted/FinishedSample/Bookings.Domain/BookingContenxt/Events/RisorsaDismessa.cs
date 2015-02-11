using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt.Events
{
    public class RisorsaDismessa : IMessage
    {
        public Guid Id { get; set; }

        public RisorsaDismessa(Guid id)
        {
            Id = id;
        }
    }
}