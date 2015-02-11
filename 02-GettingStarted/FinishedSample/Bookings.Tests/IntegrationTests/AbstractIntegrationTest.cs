using System.Configuration;
using System.Threading;
using Bookings.Domain.Messaging;
using Bookings.Domain.Support;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bookings.Tests.IntegrationTests
{
    public abstract class AbstractIntegrationTest
    {
        private Bootstrapper _bootstrapper;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var events = ConfigurationManager.ConnectionStrings["events"].ConnectionString;
            var eventsUri = new MongoUrl(events);
            var client = new MongoClient(eventsUri);
            client.GetServer().GetDatabase(eventsUri.DatabaseName).Drop();

            _bootstrapper = new Bootstrapper("events");
            _bootstrapper.Start();

            Repository = new EventStoreRepository(
                _bootstrapper.EventStore, 
                new AggregateFactory(), 
                new ConflictDetector()
            );
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _bootstrapper.Stop();
        }

		protected void WaitForPendingMessages()
	    {
			while (_bootstrapper.HasPendingMessages())
			{
				Thread.Sleep(100);
			}
	    }

		protected void SendCommand(IMessage message)
		{
			_bootstrapper.Bus.Send(message);
		}

	    protected IRepository Repository { get; private set; }
    }
}
