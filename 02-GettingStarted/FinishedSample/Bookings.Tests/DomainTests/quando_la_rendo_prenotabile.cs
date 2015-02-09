using System;
using Bookings.Domain.BookingContenxt;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa non prenotabile")]
    public class quando_la_rendo_prenotabile
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Risorsa item;

        Establish context = () =>
        {
            item = new Risorsa(_id, "MacBook Pro 13\"");
        };

        Because of = () =>
        {
            item.RendiPrenotabile();
        };

        // transizioni di stato
        It questa_diventa_prenotabile = () => item.Prenotabile.ShouldBeTrue();

        // eventi
        It l_evento_di_resa_prenotabile_e_stato_scatenato = () => item.RaisedEvent<RisorsaResaPrenotabile>().ShouldBeTrue();
    }
}