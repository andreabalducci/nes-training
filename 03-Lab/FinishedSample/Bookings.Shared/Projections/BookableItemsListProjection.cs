using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Messaging;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bookings.Shared.Projections
{
    public class BookableItemInListReadModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }

    public class BookableItemsListProjection : 
        IEventHandler<BookableItemCreated>,
        IEventHandler<BookableItemDeleted>
    {
        private MongoCollection<BookableItemInListReadModel> _collection;
        private NotifyReadModelUpdates _notifier;
        
        public BookableItemsListProjection(MongoDatabase db, NotifyReadModelUpdates notifier)
        {
            _notifier = notifier;
            _collection = db.GetCollection<BookableItemInListReadModel>("bookingList");
        }

        public void On(BookableItemCreated evt)
        {
            var item = new BookableItemInListReadModel()
                {
                    Id = evt.Id.Id,
                    Description = evt.Description
                };

            _collection.Save(item);
            _notifier.Created(item);
        }

        public void On(BookableItemDeleted evt)
        {
            var item = _collection.FindOneById(evt.Id.Id);
            _collection.Remove(Query<BookableItemInListReadModel>.Where(model => model.Id == evt.Id.Id));
            _notifier.Deleted(item);
        }
    }
}
