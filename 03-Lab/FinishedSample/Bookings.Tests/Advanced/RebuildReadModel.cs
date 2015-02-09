using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Bookings.Shared.Support;
using NEventStore;
using NEventStore.Serialization;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Bookings.Tests.Advanced
{
    [TestFixture, Explicit("lab: change projections and rebuild read model")]
    public class RebuildReadModel
    {
        private IStoreEvents _eventstore;

        [Test]
        public void rebuild_from_events()
        {
            var rebuilder = new ReplayEvents(_eventstore);
            rebuilder.ReplayAll(evt =>
                                    {
                                        Debug.WriteLine(evt.GetType());
                                        Debug.WriteLine(JsonConvert.SerializeObject(evt, Formatting.Indented));
                                        Debug.WriteLine(String.Empty);
                                    });
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _eventstore = Wireup
                .Init()
                .UsingMongoPersistence("events", new DocumentObjectSerializer())
                .Build();

            MongoDBRegistration.Register();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _eventstore.Dispose();
        }
    }
}
