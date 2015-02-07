using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SimpleEventStore.Domain;
using SimpleEventStore.Domain.Events;
using SimpleEventStore.Eventstore;
using SimpleEventStore.Query;

namespace SimpleEventStore.Tests
{
    [TestFixture]
    public class ItemTests
    {
        private const string Id = "4B8C2F8F-BDF3-47A7-B316-8CE5EFA3B33E";

        [Test]
        public void create_item()
        {
            var item = new Item(Id, "001", "SSD Crucial M4 256GB", "NR", 100);
            item.Load(100);
            item.Unload(30);

            Assert.AreEqual(3, item.Events.Count);
        }

        [Test]
        public void save_item()
        {
            var item = new Item(Id, "001", "SSD Crucial M4 256GB", "NR", 100);
            var stream = new EventStream();
            item.Save(stream);

            Assert.AreEqual(1, item.Version);
            Assert.AreEqual(1, stream.Events.Count);
        }

        [Test]
        public void load_item()
        {
            var stream = new EventStream
                             {
                                 Events =
                                     new List<object>(new[]
						                 {
						                     new ItemCreated(Id, "001", "SSD Crucial M4 256GB", "NR", 100)
						                 }),
                                 Version = 1
                             };

            var item = AggregateBase.Load<Item>(stream);

            Assert.AreEqual(1, item.Version);
            Assert.AreEqual(Id, item.Id);
        }

        [Test]
        public void save_to_disk()
        {
            object dispatchedEvent = null;
            var dispatcher = new Action<object>((evt) =>
                {
                    dispatchedEvent = evt;
                });

            var repository = new Repository(eventsDispatcher: dispatcher);

            string fname = repository.GetFileNameOfAggregateStream(Id);
            if (File.Exists(fname))
                File.Delete(fname);

            var item = new Item(Id, "001", "SSD Crucial M4 256GB", "NR", 100);
            repository.Save(item);

            Assert.IsTrue(File.Exists(fname));
            Assert.IsNotNull(dispatchedEvent);
            Assert.IsTrue(dispatchedEvent is ItemCreated);
        }

        [Test]
        public void load_from_disk()
        {
            var repository = new Repository(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests"));
            var item = repository.GetById<Item>(Id);

            Assert.IsNotNull(item);
            Assert.AreEqual(Id, item.Id);

            Assert.AreEqual(100, item.Qta);
        }

        [Test]
        public void vita_dell_articolo()
        {
            var item = new Item(Id, "ART1", "Paste per la colazione", "NR", 100);

            item.Load(100);
            item.Load(50);

            Assert.AreEqual(150, item.Qta);
            item.Unload(50);
            Assert.AreEqual(100, item.Qta);
            item.Disable();
            Assert.IsTrue(item.Disabled);

            var repository = new Repository(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests"));
            repository.Save(item);
        }


        [Test]
        public void genera_giornale_di_magazzino()
        {
            var item = new Item(Id, "ART1", "Paste per la colazione", "NR", 100);

            item.Load(100);
            item.Load(50);

            Assert.AreEqual(150, item.Qta);
            item.Unload(40);
            Assert.AreEqual(110, item.Qta);
            item.Disable();
            Assert.IsTrue(item.Disabled);

            var journal = new Journal();

            var ascolta_eventi = new Action<object>((evt) =>
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

            var repository = new Repository(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests"), ascolta_eventi);
            repository.Save(item);
        }

        [Test]
        public void test_carico()
        {
            var item = new Item();
            item.Qta = 100;
            item.Load(50);

            Assert.AreEqual(150, item.Qta);
            Assert.AreEqual(1, item.Events.Count);
            Assert.IsTrue(item.Events[0] is ItemLoaded);
        }

        [Test]
        public void genera_elenco_di_articoli()
        {
            var item = new Item(Id, "ART1", "Paste per la colazione", "NR", 90);

            item.Load(100);
            item.Unload(50);

            item.Unload(5000);

            var item2 = new Item(Guid.NewGuid().ToString(), "ART2", "Caffè", "GR", 100);

            var listaArticoli = new ItemsProjection();

            var ascolta_eventi = new Action<object>((evt) =>
            {
                var created = evt as ItemCreated;
                if (created != null)
                {
                    listaArticoli.Items.Add(new ItemModel()
                        {
                            Id = created.Id,
                            Codice = created.Code,
                            Descrizione = created.Description
                        });
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

                var disabled = evt as ItemDisabled;
                if (disabled != null)
                {
                    listaArticoli.Items.RemoveAll(x => x.Id == disabled.Id);
                }

                var sottoScorta = evt as ItemUnderMinimunAvailability;
                if (sottoScorta != null)
                {
                    var articolo = listaArticoli.Items.First(x => x.Id == sottoScorta.Id);
                    listaArticoli.ItemsUnderMinimunAvailability.Add(new ItemModel
                    {
                        Id = sottoScorta.Id,
                        Codice = articolo.Codice,
                        Descrizione = articolo.Descrizione
                    });
                }

                var unloadFailed = evt as ItemUnloadFailed;
                if (unloadFailed != null)
                {
                    var articolo = listaArticoli.Items.First(x => x.Id == unloadFailed.Id);
                    listaArticoli.FailedPickings.Add(new FailedPickingModel
                    {
                        Id = unloadFailed.Id,
                        Codice = articolo.Codice,
                        Descrizione = articolo.Descrizione,
                        QuantitàRichiesta = unloadFailed.Qta

                    });
                }
            });

            item2.Disable();

            var repository = new Repository(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests"), ascolta_eventi);
            repository.Save(item);
            repository.Save(item2);


            foreach (var articolo in listaArticoli.Items)
            {
                Debug.WriteLine("ItemModel: {0} [{1}]", articolo.Descrizione, articolo.Codice);
            }

            foreach (var articolo in listaArticoli.ItemsUnderMinimunAvailability)
            {
                Debug.WriteLine("ItemModel sottoscorta: {0} [{1}]", articolo.Descrizione, articolo.Codice);
            } 
            
            foreach (var articolo in listaArticoli.FailedPickings)
            {
                Debug.WriteLine("ItemModel sottoscorta: {0} [{1}] - Qta richiesta {2}", articolo.Descrizione, articolo.Codice , articolo.QuantitàRichiesta);
            }
        }
    }
}
