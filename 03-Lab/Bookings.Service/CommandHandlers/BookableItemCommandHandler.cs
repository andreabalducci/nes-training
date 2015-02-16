using Bookings.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using CommonDomain.Persistence;
using Rebus;

namespace Bookings.Service.CommandHandlers
{
    public class BookableItemCommandHandler : IHandleMessages<RiservaBookableItem>
    {
        public BookableItemCommandHandler(IRepository repository)
        {
            Repository = repository;
        }

        protected IRepository Repository { get; set; }

        public void Handle(RiservaBookableItem message)
        {
            BookableItem risorsa = Repository.GetById<BookableItem>(message.Itemid.Id);
            Repository.Save(risorsa, message.CommandId);
        }
    }
}