using System.Collections.Generic;

namespace SimpleEventStore.Query
{
    public class ItemsProjection
    {
        public List<ItemModel> Items { get; set; }
        public List<ItemModel> ItemsUnderMinimunAvailability { get; set; }
        public List<FailedPickingModel> FailedPickings { get; set; }

        public ItemsProjection()
        {
            Items = new List<ItemModel>();
            ItemsUnderMinimunAvailability = new List<ItemModel>();
            FailedPickings = new List<FailedPickingModel>();
        }
    }
}