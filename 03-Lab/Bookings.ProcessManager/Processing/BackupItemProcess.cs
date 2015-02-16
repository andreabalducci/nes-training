using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Shared.Domain.BookingContext.BookableItem;
using Bookings.Shared.Domain.BookingContext.BookableItem.Commands;
using Bookings.Shared.Domain.BookingContext.BookableItem.Events;
using Bookings.Shared.Integration;
using CommonDomain.Core;

namespace Bookings.ProcessManager.Processing
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
