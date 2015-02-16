using System;

namespace Bookings.Shared.Projections
{
    public class BookableItemInListReadModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}