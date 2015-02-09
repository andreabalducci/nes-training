using System;
using Bookings.Domain.BookingContenxt;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa")]
    public class quando_la_dismetto
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Risorsa item;

        Establish context = () =>
            {
                item = new Risorsa(_id, "MacBook Pro 13\"");
            };

        Because of = () =>
            {
                item.Dismetti();
            };

        // transizioni di stato
        It questa_diventa_dismessa = () => item.Dismessa.ShouldBeTrue();

        // eventi
        It l_evento_di_dismessa_e_stato_scatenato = () => item.RaisedEvent<RisorsaDismessa>().ShouldBeTrue();
    }
}