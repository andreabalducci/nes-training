using System;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Machine.Specifications;

namespace Bookings.Tests.BookingContext.BookableItemSpecs
{
    [Subject(typeof(BookableItem))]
    public class quando_un_bookableItem_è_libero : AbstractSpecification
    {
        static BookableItem Item;
        private static DateTime _da = DateTime.Today.AddDays(1);
        private static DateTime _a = DateTime.Today.AddDays(2);
        Establish context = () =>
        {
            Item = new BookableItem(new BookableItemId(Id), "Pc fisso");
        };

        Because of = () => Item.Riserva(new RichiestaDiPrenotazioneId(),  _da, _a);

        It accetta_tutto = () => Item.RaisedEvent<RiservaAccettata>().ShouldBeTrue();
    }
}