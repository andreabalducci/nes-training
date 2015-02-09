using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Machine.Specifications;

// ReSharper disable InconsistentNaming

namespace Bookings.Tests.BookingContext.BookableItemSpecs
{
    [Subject(typeof(BookableItem))]
    public abstract class AbstractSpecification
    {
        protected static Guid Id = new Guid("6B63C465-1FC4-4EF5-B6C7-EAEF36F8DF3F");
    }

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

    //[Subject(typeof(BookableItem))]
    //public class quando_un_bookableItem_è_occupato_e_disdico : AbstractSpecification
    //{
    //    static BookableItem Item;
    //    private static DateTime _da = DateTime.Today.AddDays(1);
    //    private static DateTime _a = DateTime.Today.AddDays(2);
    //    Establish context = () =>
    //    {
    //        Item = new BookableItem(new BookableItemId(Id), "Pc fisso");
    //        Item.Riserva(_da, _a);
    //    };

        

    //    Because of = () => Item.Libera();

    //    It accetta_tutto = () => Item.RaisedEvent<PrenotazioneLiberata>().ShouldBeTrue();
    //}
}
