using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Castle.Core.Logging;
using CommonDomain.Persistence;
using Rebus;
using System.Threading;

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

        static Int32 conflictCount = 0;
        public void Handle(CreateBookableItem message)
        {
            try
            {
                Logger.DebugFormat("Creo " + message.Description);

                var item = new BookableItem(message.Itemid, message.Description);
                Repository.Save(item, message.CommandId);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref conflictCount);
            }
           
        }
    }
}


