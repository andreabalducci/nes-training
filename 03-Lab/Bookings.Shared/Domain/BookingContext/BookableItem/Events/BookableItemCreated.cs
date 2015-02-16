using System;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Events
{
    public class BookableItemCreated : DomainEvent
    {
        public BookableItemId Id { get; set; }
        public string Description { get; set; }

        public BookableItemCreated(BookableItemId id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}