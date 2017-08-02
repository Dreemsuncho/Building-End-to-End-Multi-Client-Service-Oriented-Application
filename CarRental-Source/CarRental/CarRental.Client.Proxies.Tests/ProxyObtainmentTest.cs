using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Core.Common.Core;
using CarRental.Client.Bootstrapper;
using CarRental.Client.Contracts;
using Core.Common.Contracts;

namespace CarRental.Client.Proxies.Tests
{
    [TestFixture]
    public class ProxyObtainmentTest
    {
        [SetUp]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [Test]
        public void obtain_proxy_from_container_using_service_contract()
        {
            // Arrange & Act
            var proxy1 = ObjectBase.Container.GetExportedValue<IInventoryService>();
            var proxy2 = ObjectBase.Container.GetExportedValue<IRentalService>();
            var proxy3 = ObjectBase.Container.GetExportedValue<IAccountService>();

            // Assert
            Assert.IsTrue(proxy1 is InventoryClient);
            Assert.IsTrue(proxy2 is RentalClient);
            Assert.IsTrue(proxy3 is AccountClient);
        }

        [Test]
        public void obtain_proxy_from_service_factory()
        {
            // Arrange
            var serviceFactory = new ServiceFactory();

            // Act
            var proxy1 = serviceFactory.CreateClient<IInventoryService>();
            var proxy2 = serviceFactory.CreateClient<IRentalService>();
            var proxy3 = serviceFactory.CreateClient<IAccountService>();

            // Assert
            Assert.IsTrue(proxy1 is InventoryClient);
            Assert.IsTrue(proxy2 is RentalClient);
            Assert.IsTrue(proxy3 is AccountClient);
        }

        [Test]
        public void obtain_service_factory_and_proxy_from_container()
        {
            // Arrange
            var serviceFactory = ObjectBase.Container.GetExportedValue<IServiceFactory>();

            // Act
            var proxy1 = serviceFactory.CreateClient<IInventoryService>();
            var proxy2 = serviceFactory.CreateClient<IRentalService>();
            var proxy3 = serviceFactory.CreateClient<IAccountService>();

            // Assert
            Assert.IsTrue(proxy1 is InventoryClient);
            Assert.IsTrue(proxy2 is RentalClient);
            Assert.IsTrue(proxy3 is AccountClient);
        }
    }
}
