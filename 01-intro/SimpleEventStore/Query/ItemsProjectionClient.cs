using System.Collections.Generic;
using System.Linq;
using SimpleEventStore.Domain.Events;

namespace SimpleEventStore.Query
{
    public class ItemsProjectionClient
    {
        public ItemsProjectionClient()
        {
            Items = new List<ItemModel>();
            ItemsUnderMinimunAvailability = new List<ItemModel>();
            FailedPickings = new List<FailedPickingModel>();
        }

        public List<ItemModel> Items { get; set; }
        public List<ItemModel> ItemsUnderMinimunAvailability { get; set; }
        public List<FailedPickingModel> FailedPickings { get; set; }

        private void On(ItemCreated created)
        {
            Items.Add(new ItemModel
            {
                Id = created.Id,
                Sku = created.Code,
                Description = created.Description
            });
        }

        private void On(ItemBelowSafetyStockLevel sottoScorta)
        {
            var articolo = Items.First(x => x.Id == sottoScorta.Id);
            ItemsUnderMinimunAvailability.Add(new ItemModel
            {
                Id = sottoScorta.Id,
                Sku = articolo.Sku,
                Description = articolo.Description
            });
        }

        private void On(ItemDisabled disabled)
        {
            Items.RemoveAll(x => x.Id == disabled.Id);
        }

        private void On(ItemUnloadFailed unloadFailed)
        {
            var articolo = Items.First(x => x.Id == unloadFailed.Id);
            FailedPickings.Add(new FailedPickingModel
            {
                Id = unloadFailed.Id,
                Sku = articolo.Sku,
                Description = articolo.Description,
                Quantity = unloadFailed.Qta
            });
        }

        public void Observe(object evt)
        {
            var handler = GetType().GetMethod("On", new[] { evt.GetType() });
            if (handler != null)
            {
                handler.Invoke(this, new[] { evt });
            }

            //var loaded = evt as ItemLoaded;
            //if (loaded != null)
            //{
            //    var ji = journal.GetOrCreateItem(loaded.Id);
            //    ji.Total += loaded.Quantity;
            //    Debug.WriteLine("Caricato {0} con qta {1}, totale {2}", ji.Description, loaded.Quantity, ji.Total);
            //}

            //var unloaded = evt as ItemUnloaded;
            //if (unloaded != null)
            //{
            //    var ji = journal.GetOrCreateItem(unloaded.Id);
            //    ji.Total -= unloaded.Quantity;
            //    Debug.WriteLine("Scaricato {0} con qta {1}, totale {2}", ji.Description, unloaded.Quantity, ji.Total);
            //}
        }
    }
}