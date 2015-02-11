using System;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa non prenotabile")]
    public class quando_la_rendo_prenotabile
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Resource item;

        Establish context = () =>
        {
            item = new Resource(_id, "MacBook Pro 13\"");
        };

        Because of = () =>
        {
            item.MakeAvailable();
        };

        // transizioni di stato
        It questa_diventa_prenotabile = () => item.Available.ShouldBeTrue();

        // eventi
        It l_evento_di_resa_prenotabile_e_stato_scatenato = () => item.RaisedEvent<ResourceHasBeenSetAvailable>().ShouldBeTrue();
    }
}