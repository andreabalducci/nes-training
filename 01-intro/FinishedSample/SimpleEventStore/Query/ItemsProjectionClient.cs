using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using SimpleEventStore.Domain.Events;

namespace SimpleEventStore.Query
{
    public class ItemsProjectionClient
    {
        public Action<string> Log = (s) => { };
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
            Log(string.Format("Added item {0}", created.Id));
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
            Log(string.Format("Removing item {0}", disabled.Id));
            Items.RemoveAll(x => x.Id == disabled.Id);
        }

        private void On(ItemUnloadFailed unloadFailed)
        {
            var articolo = Items.First(x => x.Id == unloadFailed.Id);
            FailedPickings.Add(new FailedPickingModel
            {
                Id = unloadFailed.Id,
                Sku = articolo.Sku,
                ItemDescription = articolo.Description,
                Quantity = unloadFailed.Qta
            });
        }

        public void Observe(object evt)
        {
            var handler = GetType().GetMethod(
                "On",
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { evt.GetType() },
                null
            );

            if (handler != null)
            {
                Log(string.Format(
                    "Handled event {0}\n{1}",
                    evt.GetType().Name,
                    JsonConvert.SerializeObject(evt, Formatting.Indented)
                ));
                handler.Invoke(this, new[] { evt });
                return;
            }

            Log(string.Format(
                "Event {0} not handled",
                evt.GetType().Name
            ));

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