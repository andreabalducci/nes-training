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
        [Test]
        public void create_item()
        {
            var item = new Item(TestConfig.Id, "001", "SSD Crucial M4 256GB", "NR", 50);
            item.Load(100);
            item.Unload(30);

            Assert.AreEqual(3, item.Events.Count);
        }

        [Test]
        public void save_item_to_eventStream()
        {
            var item = new Item(TestConfig.Id, "001", "SSD Crucial M4 256GB", "NR", 100);
            var stream = new EventStream();
            item.Save(stream);

            Assert.AreEqual(1, item.Version);
            Assert.AreEqual(1, stream.Events.Count);
        }

        [Test]
        public void load_item_from_eventStream()
        {
            var stream = new EventStream
                             {
                                 Events =
                                     new List<object>(new[]
						                 {
						                     new ItemCreated(TestConfig.Id, "001", "SSD Crucial M4 256GB", "NR", 100)
						                 }),
                                 Version = 1
                             };

            var item = AggregateBase.Load<Item>(stream);

            Assert.AreEqual(1, item.Version);
            Assert.AreEqual(TestConfig.Id, item.Id);
        }

        [Test]
        public void save_to_repository()
        {
            // Arrange
            object dispatchedEvent = null;
            var dispatcher = new Action<object>((evt) =>
                {
                    dispatchedEvent = evt;
                });

            var repository = new Repository(eventsDispatcher: dispatcher);

            string fname = repository.MakeAggregateStreamFileName(TestConfig.Id);
            if (File.Exists(fname))
                File.Delete(fname);

            var item = new Item(TestConfig.Id, "001", "SSD Crucial M4 256GB", "NR", 100);

            // Act
            repository.Save(item);

            // Assert
            Assert.IsTrue(File.Exists(fname));
            Assert.IsNotNull(dispatchedEvent);
            Assert.IsTrue(dispatchedEvent is ItemCreated);
        }

        [Test]
        public void load_from_repository()
        {
            var repository = new Repository(TestConfig.PreloadedStore);
            var item = repository.GetById<Item>(TestConfig.Id);

            Assert.IsNotNull(item);
            Assert.AreEqual(TestConfig.Id, item.Id);

            Assert.AreEqual(100, item.InStock);
        }

        [Test]
        public void create_a_stream_with_few_events()
        {
            var item = new Item(TestConfig.Id, "ART1", "Paste per la colazione", "NR", 100);

            item.Load(100);
            item.Load(50);

            Assert.AreEqual(150, item.InStock);
            item.Unload(50);
            Assert.AreEqual(100, item.InStock);
            item.Disable();
            Assert.IsTrue(item.Disabled);

            var repository = new Repository(TestConfig.TestStore);
            repository.Save(item);

            // check json file
        }


        [Test]
        public void test_carico()
        {
            var item = new Item();
            item.Load(150);

            Assert.AreEqual(150, item.InStock);
            Assert.AreEqual(1, item.Events.Count);
            Assert.IsTrue(item.Events[0] is ItemLoaded);
        }

        [Test]
        public void genera_elenco_di_articoli()
        {
            var item = new Item(TestConfig.Id, "ART1", "Paste per la colazione", "NR", 90);

            item.Load(100);
            item.Unload(50);

            item.Unload(5000);

            var item2 = new Item(Guid.NewGuid().ToString(), "ART2", "Caffè", "GR", 100);

            item2.Disable();

            var projectionClient = new ItemsProjectionClient();
            var repository = new Repository(
                TestConfig.TestStore, 
                projectionClient.Observe
            );

            repository.Save(item);
            repository.Save(item2);


            foreach (var itemModel in projectionClient.Items)
            {
                Debug.WriteLine("ItemModel: {0} [{1}]", itemModel.Description, itemModel.Sku);
            }

            foreach (var itemModel in projectionClient.ItemsUnderMinimunAvailability)
            {
                Debug.WriteLine("ItemModel sottoscorta: {0} [{1}]", itemModel.Description, itemModel.Sku);
            }

            foreach (var itemModel in projectionClient.FailedPickings)
            {
                Debug.WriteLine("ItemModel sottoscorta: {0} [{1}] - InStock richiesta {2}", itemModel.Description, itemModel.Sku , itemModel.Quantity);
            }
        }
    }
}
