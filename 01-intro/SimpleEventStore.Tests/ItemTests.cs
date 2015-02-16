using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SimpleEventStore.Domain;
using SimpleEventStore.Domain.Events;
using SimpleEventStore.Eventstore;

namespace SimpleEventStore.Tests
{
    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void create_item()
        {
            var item = new Item(TestConfig.Id, "001", "SSD Crucial M4 256GB", "NR", 50);

            Assert.AreEqual(1, item.Events.Count);
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

            Assert.AreEqual(0, item.InStock);
            Assert.IsTrue(item.Disabled);
        }

        [Test]
        public void create_a_stream_with_few_events()
        {
            var item = new Item(TestConfig.Id, "SN0001", "Snacks", "NR", 100);

            item.Disable();
            Assert.IsTrue(item.Disabled);

            var repository = new Repository(TestConfig.TestStore);
            repository.Save(item);

            // check json file
        }
    }
}
