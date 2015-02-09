using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.ProcessManager.Processing;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Castle.Core.Logging;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.ProcessManager.Listeners
{
    public class BackupItemProcessListener : IHandleMessages<BookableItemCreated>
    {
        public ILogger Logger { get; set; }

        public BackupItemProcessListener(ISagaRepository repository)
        {
            Repository = repository;
        }

        private ISagaRepository Repository { get; set; }

        public void Handle(BookableItemCreated message)
        {
            Logger.Debug("Arrivato "+ message.Description);
            var process = new BackupItemProcess();
            process.Transition(message);
            Repository.Save(process, message.EventId, null);
        }
    }
}
