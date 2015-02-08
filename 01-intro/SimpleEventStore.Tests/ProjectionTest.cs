using System;
using System.Diagnostics;
using NUnit.Framework;
using SimpleEventStore.Domain;
using SimpleEventStore.Domain.Events;
using SimpleEventStore.Eventstore;
using SimpleEventStore.Query;

namespace SimpleEventStore.Tests
{
    [TestFixture]
    public class ProjectionTest
    {
        [Test]
        public void build_inventory_query_model()
        {
            var snacks = new Item(TestConfig.Id, "I0001", "Snacks", "NR", 100);

            snacks.Load(100);
            snacks.Load(50);

            Assert.AreEqual(150, snacks.InStock);
            snacks.Unload(40);
            Assert.AreEqual(110, snacks.InStock);
            snacks.Disable();
            Assert.IsTrue(snacks.Disabled);

            var journal = new Journal();

            var eventsObserver = new Action<object>((evt) =>
            {
                var created = evt as ItemCreated;
                if (created != null)
                {
                    var ji = journal.GetOrCreateItem(created.Id);
                    ji.Code = created.Code;
                    ji.Description = created.Description;
                }

                var loaded = evt as ItemLoaded;
                if (loaded != null)
                {
                    var ji = journal.GetOrCreateItem(loaded.Id);
                    ji.Total += loaded.Quantity;
                    Debug.WriteLine("Caricato {0} con qta {1}, totale {2}", ji.Description, loaded.Quantity, ji.Total);
                }

                var unloaded = evt as ItemUnloaded;
                if (unloaded != null)
                {
                    var ji = journal.GetOrCreateItem(unloaded.Id);
                    ji.Total -= unloaded.Quantity;
                    Debug.WriteLine("Scaricato {0} con qta {1}, totale {2}", ji.Description, unloaded.Quantity, ji.Total);
                }
            });

            var repository = new Repository(TestConfig.EvenstStoreFolder, eventsObserver);
            repository.Save(snacks);
        }
    }
}