using System;

namespace Bookings.Shared.Projections
{
    public class BookableItemInListReadModel
    {
        public BookableItemInListReadModel()
        {
            CreationTimestamp = DateTime.UtcNow;
        }
        public DateTime CreationTimestamp { get; set; }
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}