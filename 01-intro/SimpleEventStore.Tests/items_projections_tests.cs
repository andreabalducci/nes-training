using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using SimpleEventStore.Domain;
using SimpleEventStore.Eventstore;
using SimpleEventStore.Query;

namespace SimpleEventStore.Tests
{
    [TestFixture]
    public class items_projections_tests
    {
        [Test]
        public void feeds_items_projections()
        {
            var snacks = new Item(TestConfig.Id, "I0001", "Snacks", "NR", 90);

            snacks.Load(100);
            snacks.Unload(50);

            snacks.Unload(5000);

            var coffee = new Item(Guid.NewGuid().ToString(), "I0002", "Coffee", "KG", 100);
            coffee.Unload(10);
            coffee.Disable();

            var projectionClient = new ItemsProjectionClient()
            {
                Log = msg => Debug.WriteLine(msg)
            };

            var repository = new Repository(
                TestConfig.TestStore,
                projectionClient.Observe
            );

            repository.Save(snacks);
            repository.Save(coffee);

            Debug.WriteLine("==========================================");
            Debug.WriteLine("Items (active only)");
            Debug.WriteLine("==========================================");
            foreach (var itemModel in projectionClient.Items)
            {
                Debug.WriteLine("{0} [{1}]", itemModel.Description, itemModel.Sku);
            }

            Debug.WriteLine("");
            Debug.WriteLine("==========================================");
            Debug.WriteLine("Item below minimum availability level");
            Debug.WriteLine("==========================================");
            foreach (var itemModel in projectionClient.ItemsUnderMinimunAvailability)
            {
                Debug.WriteLine("{0} [{1}]", itemModel.Description, itemModel.Sku);
            }

            Debug.WriteLine("");
            Debug.WriteLine("==========================================");
            Debug.WriteLine("Failed pickings");
            Debug.WriteLine("==========================================");
            foreach (var failedPicking in projectionClient.FailedPickings)
            {
                Debug.WriteLine("{0} [{1}] - Requested quantity {2}", 
                    failedPicking.ItemDescription, 
                    failedPicking.Sku, 
                    failedPicking.Quantity
                );
            }


            Assert.IsTrue(projectionClient.Items.Any());
            Assert.IsTrue(projectionClient.ItemsUnderMinimunAvailability.Any());
            Assert.IsTrue(projectionClient.FailedPickings.Any());
        }
    }
}