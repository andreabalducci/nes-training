using System;
using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Commands
{
    public class DeleteBookableItem : Command
    {
        public Guid Itemid { get; set; }

        protected DeleteBookableItem()
        {
        }

        public DeleteBookableItem(Guid itemid)
        {
            Itemid = itemid;
        }
    }
}
