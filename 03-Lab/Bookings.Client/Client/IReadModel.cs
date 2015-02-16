using System.Collections.Generic;
using Bookings.Shared.Projections;

namespace Bookings.Client.Client
{
    public interface IReadModel
    {
        IList<BookableItemInListReadModel> ListItems();
    }
}