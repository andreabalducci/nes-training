using System;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa prenotabile")]
    public class quando_la_rendo_non_prenotabile
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Resource item;

        Establish context = () =>
            {
                item = new Resource(_id, "MacBook Pro 13\"");
                item.MakeAvailable();
            };

        Because of = () =>
            {
                item.MakeUnavailable();
            };

        // transizioni di stato
        It questa_diventa_non_prenotabile = () => item.Available.ShouldBeFalse();

        // eventi
        It l_evento_di_resa_non_prenotabile_e_stato_scatenato = () => item.HasRaised<ResourceHasBeenSetUnavailable>().ShouldBeTrue();
    }
}