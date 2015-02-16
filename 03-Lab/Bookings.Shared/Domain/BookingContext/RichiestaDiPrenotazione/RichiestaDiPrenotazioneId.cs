using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione
{
    public class RichiestaDiPrenotazioneId
    {
        public Guid Id { get; set; }

        public RichiestaDiPrenotazioneId()
        {
            Id = Guid.NewGuid();
        }

        public RichiestaDiPrenotazioneId(Guid id)
        {
            Id = id;
        }
    }
}
