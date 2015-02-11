using System;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Machine.Specifications;

namespace Bookings.Tests.BookingContext.BookableItemSpecs
{
    [Subject(typeof(BookableItem))]
    public class quando_un_bookableItem_è_occupato : AbstractSpecification
    {
        static BookableItem Item;
        private static DateTime _da = DateTime.Today.AddDays(1);
        private static DateTime _a = DateTime.Today.AddDays(2);
        private static RichiestaDiPrenotazioneId _richiestaDiPrenotazioneId1 = new RichiestaDiPrenotazioneId();
        private static RichiestaDiPrenotazioneId _richiestaDiPrenotazioneId2 = new RichiestaDiPrenotazioneId();

        Establish context = () =>
        {
            Item = new BookableItem(new BookableItemId(Id), "Pc fisso");
            Item.Riserva(_richiestaDiPrenotazioneId1, _da, _a);
        };

        Because of = () => Item.Riserva(_richiestaDiPrenotazioneId2, _da, _a);

        It accetta_tutto = () => Item.RaisedEvent<RiservaRifiutata>().ShouldBeTrue();
    }
}