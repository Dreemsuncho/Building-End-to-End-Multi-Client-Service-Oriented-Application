using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace CarRental.Client.Proxies.Tests
{
    [TestFixture]
    public class ServiceAccessTests
    {
        [Test]
        public void test_inventory_client_connection()
        {
            // Arrange
            var proxy = new InventoryClient();
            // Act & Assert
            proxy.Open();
        }

        [Test]
        public void test_rental_client_connection()
        {
            // Arrange
            var proxy = new RentalClient();
            // Act & Assert
            proxy.Open();
        }

        [Test]
        public void test_account_client_connection()
        {
            // Arrange
            var proxy = new AccountClient();
            // Act & Assert
            proxy.Open();
        }
    }
}
