using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor.Installer;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bookings.Tests.Storage
{
    [TestFixture, Explicit]
    public class DropAll
    {
        [Test]
        public void drop_all_databases()
        {
            MongoHelper.GetDatabase("rebus").Drop();
            MongoHelper.GetDatabase("events").Drop();
            MongoHelper.GetDatabase("readmodel").Drop();
            MongoHelper.GetDatabase("pm").Drop();
        }
    }

    public static class MongoHelper
    {
        public static MongoDatabase GetDatabase(string cstringName)
        {
            var cstring = ConfigurationManager.ConnectionStrings[cstringName].ConnectionString;
            var mongoUrl = new MongoUrl(cstring);
            var server = new MongoClient(mongoUrl).GetServer();
            return server.GetDatabase(mongoUrl.DatabaseName, new MongoDatabaseSettings());
        }
    }
}
