using System;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
    public class BookableItemDeleted : DomainEvent
    {
        public BookableItemId Id { get; set; }

        public BookableItemDeleted(BookableItemId id)
        {
            if (id == null) throw new ArgumentNullException("id");
            Id = id;
        }
    }
}