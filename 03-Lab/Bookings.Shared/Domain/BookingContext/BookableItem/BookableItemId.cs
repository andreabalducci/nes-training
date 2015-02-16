using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared.Domain.BookingContext.BookableItem
{
    public class BookableItemId
    {
        public Guid Id { get; set; }

        public BookableItemId()
        {
            
        }

        public BookableItemId(Guid id)
        {
            Id = id;
        }
    }
}
