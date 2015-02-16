using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor.Installer;
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
}
