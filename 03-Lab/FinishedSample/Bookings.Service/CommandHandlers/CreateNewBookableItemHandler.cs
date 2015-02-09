using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Domain.BookingContext.RichiestaDiPrenotazione;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.RichiestaDiPrenotazione.Commands;
using Castle.Core.Logging;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.Service.CommandHandlers
{
    public class CreateNewBookableItemHandler : IHandleMessages<CreateBookableItem>
    {
        public ILogger Logger { get; set; }
        private IRepository Repository { get; set; }

        public CreateNewBookableItemHandler(IRepository repository)
        {
            Repository = repository;
        }

        public void Handle(CreateBookableItem message)
        {
            Logger.DebugFormat("Creo "+message.Description);

            var item = new BookableItem(message.Itemid, message.Description);
            Repository.Save(item, message.CommandId);
        }
    }


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


