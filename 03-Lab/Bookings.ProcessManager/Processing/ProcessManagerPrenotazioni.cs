using System.Collections.Generic;
using System.Linq;
using Bookings.ProcessManager.Messaggi;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Events;
using CommonDomain.Core;

namespace Bookings.ProcessManager.Processing
{
    public class ProcessManagerPrenotazioni : SagaBase<object>
    {
        public ProcessManagerPrenotazioni()
        {
            Register<RichiestaDiPrenotazioneApprovata>(On);
            Register<RiservaRifiutata>(On);
            Register<TimeoutApprovazione>(On);
        }

        private void On(TimeoutApprovazione obj)
        {
            var comando = new RespingiRichiesta(obj.Id);
            Dispatch(comando);
        }

        private void On(RichiestaDiPrenotazioneApprovata evt)
        {
            this.Id = evt.Id.Id.ToString();
            var comando = new RiservaBookableItem(evt.BookableItemId, evt.Da, evt.A, evt.Id);
            
            Dispatch(comando);
            Dispatch(new TimeoutApprovazione(evt.Id));
        }

        private void On(RiservaRifiutata evt)
        {
            var comando = new RespingiRichiesta(evt.RichiestaDiPrenotazioneId);
            Dispatch(comando);
        }
    }
}
