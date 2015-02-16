using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.Service.CommandHandlers
{
    public class ApprovaPrenotazioneCommandHandler
        : IHandleMessages<RichiediApprovazionePrenotazione>
        , IHandleMessages<RespingiRichiesta>

    {
        public ApprovaPrenotazioneCommandHandler(IRepository repository)
        {
            Repository = repository;
        }

        protected IRepository Repository { get; set; }

        public void Handle(RichiediApprovazionePrenotazione message)
        {
            var rdp = Repository.GetById<RichiestaDiPrenotazione>(message.IdPrenotazione.Id);
            rdp.Approva();
            Repository.Save(rdp, message.CommandId);
        }

        public void Handle(RespingiRichiesta message)
        {
            var rdp = Repository.GetById<RichiestaDiPrenotazione>(message.RichiestaDiPrenotazioneId.Id);
            rdp.Respingi();
            Repository.Save(rdp, message.CommandId);
        }
    }
}
