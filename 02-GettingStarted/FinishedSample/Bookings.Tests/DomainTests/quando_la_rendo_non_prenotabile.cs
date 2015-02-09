using System;
using Bookings.Domain.BookingContenxt;
using Machine.Specifications;

namespace Bookings.Tests.DomainTests
{
    [Subject("Risorsa prenotabile")]
    public class quando_la_rendo_non_prenotabile
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Risorsa item;

        Establish context = () =>
            {
                item = new Risorsa(_id, "MacBook Pro 13\"");
                item.RendiPrenotabile();
            };

        Because of = () =>
            {
                item.RendiNonPrenotabile();
            };

        // transizioni di stato
        It questa_diventa_non_prenotabile = () => item.Prenotabile.ShouldBeFalse();

        // eventi
        It l_evento_di_resa_non_prenotabile_e_stato_scatenato = () => item.RaisedEvent<RisorsaResaNonPrenotabile>().ShouldBeTrue();
    }
}