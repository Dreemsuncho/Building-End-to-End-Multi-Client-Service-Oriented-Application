using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Core.Common.Core;
using CarRental.Business.Bootstrapper;

namespace CarRental.Data.Tests
{
    [TestFixture]
    public class DataLayerIntegration_Tests
    {
        [SetUp]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [Test]
        public void test_repository_usage()
        {
            // Arrange
            var repository = new RepositoryTestClass();

            // Act
            var cars = repository.GetCars();

            // Assert
            Assert.IsNotNull(cars);
        }

        [Test]
        public void test_repository_factory_usage()
        {
            // Arrange
            var repositoryFactory = new RepositoryFactoryTestClass();

            // Act
            var cars = repositoryFactory.GetCars();

            // Assert
            Assert.IsNotNull(cars);
        }
    }
}
