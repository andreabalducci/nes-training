using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bookings.Domain.Support;
using CommonDomain.Persistence.EventStore;
using NEventStore;
using NEventStore.Serialization;
using NUnit.Framework;

namespace Bookings.Tests.NESTests
{
    [TestFixture]
    public class EvenstreamTests
    {
        private IStoreEvents _store;
        private string _inventoryBucket = "inventory";

        public class ItemCreatedEvent
        {
            public string Id { get; set; }
        }

        public class ItemDisabledEvent
        {
        }

        public class ItemSKUChanged
        {
            public string SKU { get; set; }
        }

        [SetUp]
        public void TestFixtureSetUp()
        {
            MongoHelper.DropAll();
            _store = CreateStore();
        }

        [TearDown]
        public void TestFixtureTearDown()
        {
            _store.Dispose();
        }

        [Test]
        public void create_a_stream_with_a_new_itemCreatedEvent()
        {
            CreateItem("Item_1", "ABC00123");
        }

        [Test]
        public void create_and_disable_Item_1()
        {
            CreateItem("Item_1", "ABC00123");
            DisableItem("Item_1");
        }

        [Test]
        public void create_and_disable_Item_1_and_Item_2()
        {
            CreateItem("Item_1", "ABC00123");
            CreateItem("Item_2", "xxxxxxx");
            DisableItem("Item_2");
            DisableItem("Item_1");
        }

        private void DisableItem(string itemId)
        {
            using (var stream = _store.OpenStream(_inventoryBucket, itemId, 0, int.MaxValue))
            {
                stream.Add(new EventMessage()
                {
                    Body = new ItemDisabledEvent()
                });

                stream.CommitChanges(commitId: Guid.NewGuid());
            }
        }

        private void CreateItem(string itemId, string sku)
        {
            using (var stream = _store.CreateStream(_inventoryBucket, itemId))
            {
                stream.Add(new EventMessage()
                {
                    Headers = new Dictionary<string, object>(){{"secret","value"}},
                    Body = new ItemCreatedEvent()
                    {
                        Id = itemId
                    }
                });

                stream.Add(new EventMessage()
                {
                    Body = new ItemSKUChanged()
                    {
                        SKU = sku
                    }
                });

                FillDiagnosticHeaders(stream.UncommittedHeaders);
                
                stream.CommitChanges(commitId: Guid.NewGuid());
            }
        }

        private static void FillDiagnosticHeaders(IDictionary<string, object> headers)
        {
            headers.Add("author", "test");
            headers.Add("machine", Environment.MachineName);
            headers.Add("app_name", Assembly.GetExecutingAssembly().FullName);
            headers.Add("app_ver", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private static IStoreEvents CreateStore()
        {
            var store = Wireup.Init()
                .UsingMongoPersistence("events", new DocumentObjectSerializer())
                .InitializeStorageEngine()
                .DoNotDispatchCommits()
                .Build();
            return store;
        }
    }
}
