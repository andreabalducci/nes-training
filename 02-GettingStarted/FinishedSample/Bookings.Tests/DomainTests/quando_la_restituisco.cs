using System;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa")]
    public class quando_la_restituisco
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Resource item;

        Establish context = () =>
            {
                item = new Resource(_id, "MacBook Pro 13\"");
                item.Lend();
            };

        Because of = () =>
            {
                item.Return();
            };

        // transizioni di stato
        It questa_diventa_non_presa = () => item.Lent.ShouldBeFalse();

        // eventi
        It l_evento_di_restituita_e_stato_scatenato = () => item.RaisedEvent<ResourceReturned>().ShouldBeTrue();
    }
}