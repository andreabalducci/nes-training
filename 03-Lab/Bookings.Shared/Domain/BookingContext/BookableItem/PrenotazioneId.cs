using System;

namespace Bookings.Shared.Domain.BookingContext.BookableItem
{
    public class PrenotazioneId
    {
        protected PrenotazioneId()
        {
        }

        public Guid Id { get; set; }
        public BookableItemId BookableItemId { get; set; }

        public PrenotazioneId(Guid id, BookableItemId bookableItemId)
        {
            Id = id;
            BookableItemId = bookableItemId;
        }
    }
}