using System.Configuration;
using System.Threading;
using Bookings.Domain.Messaging;
using Bookings.Domain.Support;
using CommonDomain.Persistence;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bookings.Tests.IntegrationTests
{
    public abstract class AbstractIntegrationTest
    {
        private IBookingApplication _application;
        protected IRepository Repository { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // clenaup
            DropDb();

            // startup
            _application = new Bootstrapper("events").Start();
            Repository = _application.CreateRepository();
        }

        private static void DropDb()
        {
            var events = ConfigurationManager.ConnectionStrings["events"].ConnectionString;
            var eventsUri = new MongoUrl(events);
            var client = new MongoClient(eventsUri);
            client.GetServer().GetDatabase(eventsUri.DatabaseName).Drop();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _application.Stop();
        }

		protected void FlushExecutionQueue()
	    {
            _application.FlushExecutionQueue();
        }

		protected void SendCommand(IMessage message)
		{
			_application.Accept(message);
		}
    }
}
