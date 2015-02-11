using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using Machine.Specifications;

namespace Bookings.Tests.BookingContext.RichiestaDiPrenotazioneSpecs
{
    public class quando_una_richiesta_di_prenotazione_viene_creata
    {
        private static RichiestaDiPrenotazione _rdp;
        private static RichiestaDiPrenotazioneId _idRichiesta = new RichiestaDiPrenotazioneId(new Guid("A9CE6557-6F19-466A-A23F-36C080079A76"));
        private static BookableItemId _biId = new BookableItemId(new Guid("FC7F00FE-6762-438D-AC2F-4E7D49B4CCF9"));
        private static string _utente = "Stefano";
        private static DateTime _da = new DateTime(2013, 09, 18, 8, 0, 0);
        private static DateTime _a = new DateTime(2013, 09, 20, 18, 30, 0);
        private static string _causale = "Corso CQRS";

        private Because of = () => _rdp = new RichiestaDiPrenotazione(_idRichiesta, _biId, _utente, _da, _a, _causale);

        It l_evento_di_creazione_è_stato_lanciato = () => _rdp.RaisedEvent<RichiestaDiPrenotazioneCreata>();
    }
}
