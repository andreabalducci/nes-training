using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Castle.Core.Logging;
using NEventStore.Domain.Persistence;
using Rebus;

namespace Bookings.Service.CommandHandlers
{
    public class DeleteBookableItemCommandHandler : IHandleMessages<DeleteBookableItem>
    {
        public ILogger Logger { get; set; }
        private IRepository Repository { get; set; }

        public DeleteBookableItemCommandHandler(IRepository repository)
        {
            Repository = repository;
        }

        public void Handle(DeleteBookableItem message)
        {
            var item = Repository.GetById<BookableItem>(message.Itemid);
            
            // why?
            if(item.Version == 0)
                throw new Exception("Invalid item id");

            item.Delete();

            Repository.Save(item, message.CommandId);
        }
    }
}
