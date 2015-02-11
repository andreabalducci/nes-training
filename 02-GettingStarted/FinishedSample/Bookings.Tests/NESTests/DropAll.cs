using NUnit.Framework;

namespace Bookings.Tests.NESTests
{
    [TestFixture, Explicit]
    public class DropAll
    {
        [Test]
        public void drop_all_databases()
        {
            MongoHelper.DropAll();
        }
    }
}
