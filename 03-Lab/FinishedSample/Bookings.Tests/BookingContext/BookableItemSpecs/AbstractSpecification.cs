using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.BookableItem;
using Machine.Specifications;

// ReSharper disable InconsistentNaming

namespace Bookings.Tests.BookingContext.BookableItemSpecs
{
    [Subject(typeof(BookableItem))]
    public abstract class AbstractSpecification
    {
        protected static Guid Id = new Guid("6B63C465-1FC4-4EF5-B6C7-EAEF36F8DF3F");
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

    //    It accetta_tutto = () => Item.HasRaised<PrenotazioneLiberata>().ShouldBeTrue();
    //}
}
