using System;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
    public class RiservaAccettata : DomainEvent
    {
        protected RiservaAccettata()
        {
        }

        public BookableItemId IdRisorsa { get; set; }
        public DateTime Da { get; set; }
        public DateTime A { get; set; }

        public RiservaAccettata(BookableItemId id, DateTime da, DateTime a)
        {
            IdRisorsa = id;
            Da = da;
            A = a;
        }
    }
}