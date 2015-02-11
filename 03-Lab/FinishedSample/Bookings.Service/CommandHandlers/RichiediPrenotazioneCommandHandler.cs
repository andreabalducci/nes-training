using Bookings.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.Service.CommandHandlers
{
    public class RichiediPrenotazioneCommandHandler : IHandleMessages<CreaRichiestaDiPrenotazione>
    {
        public RichiediPrenotazioneCommandHandler(IRepository repository)
        {
            Repository = repository;
        }

        protected IRepository Repository { get; set; }

        public void Handle(CreaRichiestaDiPrenotazione message)
        {
            RichiestaDiPrenotazione richiestaDiPrenotazione = new RichiestaDiPrenotazione(message.RichiestaDiPrenotazioneId, message.Itemid,message.Utente, message.Da,message.A, message.Causale);
            
            Repository.Save(richiestaDiPrenotazione, message.CommandId);
        }
    }
}