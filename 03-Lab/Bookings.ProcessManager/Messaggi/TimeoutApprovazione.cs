using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;

namespace Bookings.ProcessManager.Messaggi
{
    public class TimeoutApprovazione
    {
        public RichiestaDiPrenotazioneId Id { get; set; }
        public TimeSpan Delay { get; set; }
        public Guid TimeoutId { get; set; }

        public TimeoutApprovazione()
        {
        
        }

        public TimeoutApprovazione(RichiestaDiPrenotazioneId id)
        {
            this.TimeoutId = Guid.NewGuid();
            Id = id;
            this.Delay = TimeSpan.FromMinutes(2);
        }
    }
}
