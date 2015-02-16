using System.Configuration;
using MongoDB.Driver;

namespace Bookings.Tests.Storage
{
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