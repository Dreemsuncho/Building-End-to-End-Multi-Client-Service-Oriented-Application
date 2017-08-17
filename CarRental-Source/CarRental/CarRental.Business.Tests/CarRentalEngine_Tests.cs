using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using Core.Common.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;

namespace CarRental.Business.Tests
{
    [TestFixture]
    public class CarRentalEngine_Tests
    {
        [Test]
        public void IsCarCurrentlyRented_any_account()
        {
            // Arrange
            var rental = new Rental
            {
                CarId = 1
            };

            var rentalRepository = new Mock<IRentalRepository>();
            var dataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            rentalRepository.Setup(x => x.GetCurrentRentalByCar(1)).Returns(rental);
            dataRepositoryFactory.Setup(x => x.GetDataRepository<IRentalRepository>()).Returns(rentalRepository.Object);

            var carRentalEngine = new CarRentalEngine(dataRepositoryFactory.Object);

            // Act
            bool actual1 = carRentalEngine.IsCarCurrentlyRented(1);
            bool actual2 = carRentalEngine.IsCarCurrentlyRented(2);

            // Assert
            Assert.IsTrue(actual1);
            Assert.IsFalse(actual2);
        }
    }
}
