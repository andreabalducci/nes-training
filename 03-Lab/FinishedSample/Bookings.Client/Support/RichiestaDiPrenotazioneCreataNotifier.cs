using System;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using Rebus;

namespace Bookings.Client.Support
{
    public class RichiestaDiPrenotazioneCreataNotifier : 
        IHandleMessages<RichiestaDiPrenotazioneCreata>,
        IHandleMessages<RichiestaDiPrenotazioneApprovata>
    {
        public void Handle(RichiestaDiPrenotazioneCreata message)
        {
            Console.WriteLine("Richiesta di '{0}' creata!", message.Utente);
        }

        public void Handle(RichiestaDiPrenotazioneApprovata message)
        {
            Console.WriteLine("Richiesta di prenotazione '{0}' approvata!", message.Id);
        }
    }
}