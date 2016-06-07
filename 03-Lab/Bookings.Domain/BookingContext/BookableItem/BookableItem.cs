using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using NEventStore.Domain.Core;

namespace Bookings.Domain.BookingContext.BookableItem
{
    public class BookableItem : AggregateBase
    {
        protected bool Deleted { get; set; }
        private BookableItemId _id;

        protected BookableItem()
        {
        }

        public BookableItem(BookableItemId id, string description)
            : this()
        {
            RaiseEvent(new BookableItemCreated(id, description));
        }

        public void Delete()
        {
            RaiseEvent(new BookableItemDeleted(_id));
        }

        public void Apply(BookableItemCreated evt)
        {
            _id = evt.Id;
            Id = evt.Id.Id;
        }

        public void Apply(BookableItemDeleted evt)
        {
            this.Deleted = true;
        }
    }
}
