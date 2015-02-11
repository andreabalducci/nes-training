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
        private IBookingApplication _application;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var events = ConfigurationManager.ConnectionStrings["events"].ConnectionString;
            var eventsUri = new MongoUrl(events);
            var client = new MongoClient(eventsUri);
            client.GetServer().GetDatabase(eventsUri.DatabaseName).Drop();

            _application = new Bootstrapper("events").Start();
            Repository = _application.CreateRepository();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _application.Stop();
        }

		protected void WaitForPendingMessages()
	    {
            while (_application.HasPendingMessages)
			{
				Thread.Sleep(100);
			}
	    }

		protected void SendCommand(IMessage message)
		{
			_application.Accept(message);
		}

	    protected IRepository Repository { get; private set; }
    }
}
