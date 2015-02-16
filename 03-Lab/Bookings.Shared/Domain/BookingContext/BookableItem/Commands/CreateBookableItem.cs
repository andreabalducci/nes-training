using Bookings.Shared.Messaging;

namespace Bookings.Shared.Domain.BookingContext.BookableItem.Commands
{
    public class CreateBookableItem : Command
    {
        public BookableItemId Itemid { get; set; }
        public string Description { get; set; }

        protected CreateBookableItem()
        {
        }

        public CreateBookableItem(BookableItemId itemid, string description)
        {
            this.Itemid = itemid;
            this.Description = description;
        }
    }
}
