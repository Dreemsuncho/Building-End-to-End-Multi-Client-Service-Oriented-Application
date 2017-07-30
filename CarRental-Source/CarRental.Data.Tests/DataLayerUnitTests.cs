using System.Collections.Generic;

using NUnit.Framework;
using Moq;

using Core.Common.Contracts;
using CarRental.Data.Contracts;
using CarRental.Business.Entities;

namespace CarRental.Data.Tests
{
    [TestFixture]
    public class DataLayerUnitTests
    {
        [Test]
        public void test_repository_mocking()
        {
            // Arrange
            var mockCarRepository = new Mock<ICarRepository>();
            var repositoryTestClass = new RepositoryTestClass(mockCarRepository.Object);

            var cars = new List<Car>
            {
                new Car { CarId = 1, Description = "Car1" },
                new Car { CarId = 2, Description = "Car2" }
            };

            mockCarRepository.Setup(x => x.Get()).Returns(cars);

            // Act
            var actual = repositoryTestClass.GetCars();

            // Assert
            Assert.AreSame(cars, actual);
        }

        [Test]
        public void test_repository_factory_mocking()
        {
            // Arrange
            var repositoryFactory = new Mock<IDataRepositoryFactory>();

            var cars = new List<Car>
            {
                new Car { CarId = 1, Description = "Car1" },
                new Car { CarId = 2, Description = "Car2" }
            };

            repositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>().Get()).Returns(cars);

            var repositoryFactoryTest = new RepositoryFactoryTestClass(repositoryFactory.Object);
            // Act
            var actual = repositoryFactoryTest.GetCars();

            // Assert
            Assert.AreSame(cars, actual);
        }
    }
}
