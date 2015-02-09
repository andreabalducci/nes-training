using Bookings.ProcessManager.Messaggi;
using Bookings.ProcessManager.Processing;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.ProcessManager.Listeners
{
    public class ProcessManagerPrenotazioniListener 
        : IHandleMessages<RichiestaDiPrenotazioneApprovata>
        , IHandleMessages<RiservaRifiutata>
        , IHandleMessages<RiservaAccettata>
        , IHandleMessages<TimeoutApprovazione>
    {
        public ISagaRepository Repository { get; set; }

        public ProcessManagerPrenotazioniListener(ISagaRepository repository)
        {
            Repository = repository;
        }

        public void Handle(RichiestaDiPrenotazioneApprovata message)
        {
            var process = new ProcessManagerPrenotazioni();
            process.Transition(message);
            Repository.Save(process, message.EventId, null);    
        }

        public void Handle(RiservaRifiutata message)
        {
            var process = Repository.GetById<ProcessManagerPrenotazioni>(message.RichiestaDiPrenotazioneId.Id);
            process.Transition(message);
            Repository.Save(process, message.EventId, null);
        }

        public void Handle(RiservaAccettata message)
        {
            
        }

        public void Handle(TimeoutApprovazione message)
        {
            var process = Repository.GetById<ProcessManagerPrenotazioni>(message.Id.Id);
            process.Transition(message);
            Repository.Save(process, message.TimeoutId, null);
        }
    }
}
