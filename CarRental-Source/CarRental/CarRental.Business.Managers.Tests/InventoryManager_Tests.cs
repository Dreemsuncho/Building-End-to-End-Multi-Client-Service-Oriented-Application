using System.Collections.Generic;
using System.Threading;
using System.Security.Principal;
using System.ServiceModel;

using NUnit.Framework;
using Moq;

using Core.Common.Exceptions;
using Core.Common.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using CarRental.Business.Common;
using System;
using CarRental.Common;

namespace CarRental.Business.Managers.Tests
{
    [TestFixture]
    public class InventoryManager_Tests
    {
        [SetUp]
        public void Initialize()
        {
            Security.AddGenericPrincipal();
        }

        [Test]
        public void UpdateCar_add_new()
        {
            // Arrange
            var newCar = new Car();
            var addedCar = new Car { CarId = 1 };

            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);

            // Act
            var actual = inventoryManager.UpdateCar(newCar);

            // Assert
            Assert.AreSame(addedCar, actual);
        }

        [Test]
        public void UpdateCar_update_existing()
        {
            // Arrange
            var existingCar = new Car { CarId = 1 };
            var updatedCar = new Car { CarId = 1 };

            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);

            // Act
            var actual = inventoryManager.UpdateCar(existingCar);

            // Assert
            Assert.AreSame(updatedCar, actual);
        }

        [Test]
        public void DeleteCar()
        {
            // Arrange
            var carRepository = new Mock<ICarRepository>();
            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>()).Returns(carRepository.Object);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);

            // Act
            inventoryManager.DeleteCar(1);

            // Assert
            dataRepositoryFactory.Verify(x => x.GetDataRepository<ICarRepository>(), Times.Once);
            carRepository.Verify(x => x.Remove(1), Times.Once);
        }

        [Test]
        public void GetCar()
        {
            // Arrange
            var car = new Car { CarId = 1 };
            var carRepository = new Mock<ICarRepository>();
            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>()).Returns(carRepository.Object);
            carRepository.Setup(x => x.Get(1)).Returns(car);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);

            // Act
            var actual = inventoryManager.GetCar(1);

            // Assert
            dataRepositoryFactory.Verify(x => x.GetDataRepository<ICarRepository>(), Times.Once);
            carRepository.Verify(x => x.Get(1), Times.Once);
            Assert.AreSame(car, actual);
        }

        [Test]
        public void GetCar_throws()
        {
            // Arrange
            var carRepository = new Mock<ICarRepository>();
            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>()).Returns(carRepository.Object);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);
            int carId = 1;

            // Act & Assert
            var ex = Assert.Throws<FaultException<NotFoundException>>(() => inventoryManager.GetCar(1));
            Assert.AreEqual($"Car with ID of {carId} is not in database", ex.Message);
        }

        [Test]
        public void GetAllCars()
        {
            // Arrange
            var car = new Car();
            var cars = new List<Car> { car };
            var rentedCars = new List<Rental>();
            var carRepository = new Mock<ICarRepository>();
            var rentalRepository = new Mock<IRentalRepository>();

            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            carRepository.Setup(x => x.Get()).Returns(cars);
            rentalRepository.Setup(x => x.GetCurrentlyRentedCars()).Returns(rentedCars);
            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>()).Returns(carRepository.Object);
            dataRepositoryFactory.Setup(x => x.GetDataRepository<IRentalRepository>()).Returns(rentalRepository.Object);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object);

            // Act
            var collection = inventoryManager.GetAllCars();

            // Assert
            dataRepositoryFactory.Verify(x => x.GetDataRepository<ICarRepository>(), Times.Once);
            dataRepositoryFactory.Verify(x => x.GetDataRepository<IRentalRepository>(), Times.Once);
            carRepository.Verify(x => x.Get(), Times.Once);
            rentalRepository.Verify(x => x.GetCurrentlyRentedCars(), Times.Once);
            CollectionAssert.Contains(collection, car);
        }

        [Test]
        public void GetAvailableCars()
        {
            // Arrange
            var car = new Car();
            var cars = new List<Car> { car, car, car, car };
            var pickupDate = DateTime.Now;
            var returnDate = DateTime.Now.AddDays(1);

            var carRepository = new Mock<ICarRepository>();
            var rentalRepository = new Mock<IRentalRepository>();
            var reservationRepository = new Mock<IReservationRepository>();

            var carRentalEngine = new Mock<ICarRentalEngine>();

            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            var businessEngineFactory = new Mock<IBusinessEngineFactory>();

            dataRepositoryFactory.Setup(x => x.GetDataRepository<ICarRepository>()).Returns(carRepository.Object);
            dataRepositoryFactory.Setup(x => x.GetDataRepository<IRentalRepository>()).Returns(rentalRepository.Object);
            dataRepositoryFactory.Setup(x => x.GetDataRepository<IReservationRepository>()).Returns(reservationRepository.Object);

            businessEngineFactory.Setup(x => x.GetBusinessEngine<ICarRentalEngine>()).Returns(carRentalEngine.Object);

            carRepository.Setup(x => x.Get()).Returns(cars);
            carRentalEngine.Setup(x => x.IsCarAvailableForRental(car.CarId, pickupDate, returnDate, It.IsAny<IEnumerable<Rental>>(), It.IsAny<IEnumerable<Reservation>>())).Returns(true);

            var inventoryManager = new InventoryManager(dataRepositoryFactory.Object, businessEngineFactory.Object);

            // Act
            var result = inventoryManager.GetAvailableCars(pickupDate, returnDate);

            // Assert
            dataRepositoryFactory.Verify(x => x.GetDataRepository<ICarRepository>(), Times.Once);
            dataRepositoryFactory.Verify(x => x.GetDataRepository<IRentalRepository>(), Times.Once);
            dataRepositoryFactory.Verify(x => x.GetDataRepository<IReservationRepository>(), Times.Once);
            businessEngineFactory.Verify(x => x.GetBusinessEngine<ICarRentalEngine>(), Times.Once);
            carRepository.Verify(x => x.Get(), Times.Once);
            rentalRepository.Verify(x => x.GetCurrentlyRentedCars(), Times.Once);
            reservationRepository.Verify(x => x.Get(), Times.Once);
            carRentalEngine.Verify(x => x.IsCarAvailableForRental(car.CarId, pickupDate, returnDate, It.IsAny<IEnumerable<Rental>>(), It.IsAny<IEnumerable<Reservation>>()), Times.Exactly(4));
            CollectionAssert.Contains(result, car);
            Assert.IsTrue(result.Length == 4);
        }
    }
}
