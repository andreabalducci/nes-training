using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Machine.Specifications;

namespace Bookings.Tests.BookingContext.BookableItemSpecs
{
    [Subject(typeof(BookableItem))]
    public class when_a_bookableItem_is_created : AbstractSpecification
    {
        static BookableItem Item;
        Establish context = () => { Item = null; };
        Because of = () => Item = new BookableItem(new BookableItemId(Id), "a brand new item");
        It the_id_should_be_set = () => Item.Id.ShouldBeLike(Id);
        It the_create_event_should_have_been_raised =
            () => Item.RaisedEvent<BookableItemCreated>().ShouldBeTrue();
        It createdEvent_should_have_item_description_set =
            () => Item.LastEventOfType<BookableItemCreated>().Description.ShouldBeLike("a brand new item");
    }
}