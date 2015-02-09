using System;
using Bookings.Domain.Messaging;

namespace Bookings.Domain.BookingContenxt
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