using System;
using Bookings.Domain.BookingContenxt;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa")]
    public class quando_la_prendo
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Risorsa item;

        Establish context = () =>
            {
                item = new Risorsa(_id, "MacBook Pro 13\"");
            };

        Because of = () =>
            {
                item.Prendi();
            };

        // transizioni di stato
        It questa_diventa_presa = () => item.Presa.ShouldBeTrue();

        // eventi
        It l_evento_di_presa_e_stato_scatenato = () => item.RaisedEvent<RisorsaPresa>().ShouldBeTrue();
    }
}