using System;
using System.Collections.Generic;
using System.Text;
using Bookings.Domain.BookingContenxt;
using Bookings.Domain.BookingContenxt.Events;
using Machine.Specifications;

// ReSharper disable InconsistentNaming

namespace Bookings.Tests.DomainTests
{
    [Subject(typeof(Resource))]
    public class quando_creo_una_risorsa
    {
        static readonly Guid _id = new Guid("4534C386-5284-4203-9AA3-87B60A172764");
        static Resource item;

        Establish context = () =>
        {
            item = null;
        };

        Because of = () =>
        {
            item = new Resource(_id, "MacBook Pro 13\"");
        };

        // transizioni di stato
        It ha_un_identificativo = () => item.Id.ShouldBeLike(_id);
        It la_descrizione_impostata = () => item.Description.ShouldBeLike("MacBook Pro 13\"");
        It questa_non_è_prenotabile = () => item.Available.ShouldBeFalse();

        // eventi
        It l_evento_di_crazione_e_stato_scatenato = () => item.RaisedEvent<ResourceCreated>().ShouldBeTrue();
    }
}
