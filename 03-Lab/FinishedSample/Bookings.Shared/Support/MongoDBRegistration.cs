using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bookings.Shared.Messaging;
using MongoDB.Bson.Serialization;

namespace Bookings.Shared.Support
{
    public static class MongoDBRegistration
    {
        public static void Register()
        {
            var types =
                Assembly.GetExecutingAssembly().GetTypes()
                                     .Where(x => x.IsClass && !x.IsAbstract &&
                                         (
                                            typeof(IDomainEvent).IsAssignableFrom(x) ||
                                            typeof(Command).IsAssignableFrom(x)
                                         )
            );

            foreach (var type in types)
            {
                BsonClassMap.LookupClassMap(type);
            }
        }
    }
}
