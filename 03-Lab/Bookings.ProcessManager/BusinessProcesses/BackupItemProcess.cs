using System;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using NEventStore.Domain.Core;

namespace Bookings.ProcessManager.BusinessProcesses
{
    public class BackupItemProcess : SagaBase<object>
    {
        public BackupItemProcess()
        {
            Register<BookableItemCreated>(ItemCreated);
        }

        private void ItemCreated(BookableItemCreated obj)
        {
            Id = obj.Id.Id.ToString();
/*
            Dispatch(new PublishItemToIntranet()
                {
                    ItemId =  obj.Id,
                    Description = obj.Description
                });
*/
            if (obj.Description.Contains("+backup"))
            {
                Dispatch(new CreateBookableItem(new BookableItemId(Guid.NewGuid()), "Backup of "+ obj.Description.Replace("+backup", "")));
            }
        }
    }
}
