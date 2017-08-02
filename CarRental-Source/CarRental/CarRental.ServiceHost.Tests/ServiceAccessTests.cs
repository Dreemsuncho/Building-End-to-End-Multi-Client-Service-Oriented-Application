using System.ServiceModel;

using NUnit.Framework;
using CarRental.Business.Contracts;

namespace CarRental.ServiceHost.Tests
{
    [TestFixture]
    public class ServiceAccessTests
    {
        [Test]
        public void test_inventory_manager_as_service()
        {
            // Arange
            var channelFactory = new ChannelFactory<IInventoryService>("");

            var proxy = channelFactory.CreateChannel();

            // Act & Assert
            (proxy as ICommunicationObject).Open();
        }

        [Test]
        public void test_rental_manager_as_service()
        {
            // Arange
            var channelFactory = new ChannelFactory<IRentalService>("");

            var proxy = channelFactory.CreateChannel();

            // Act & Assert
            (proxy as ICommunicationObject).Open();
        }

        [Test]
        public void test_account_manager_as_service()
        {
            // Arange
            var channelFactory = new ChannelFactory<IAccountService>("");

            var proxy = channelFactory.CreateChannel();

            // Act & Assert
            (proxy as ICommunicationObject).Open();
        }
    }
}
