using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net;

using NUnit.Framework;
using Moq;

using CarRental.Client.Contracts;
using CarRental.Web.Controllers;
using CarRental.Client.Entities;
using CarRental.Web.Models;

namespace CarRental.Web.Tests.Controllers
{
    [TestFixture]
    public class ReservationApiController_Tests : ApiControllerTestClass
    {
        [Test]
        public void GetAvailableCars()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            Car[] cars = { new Car { CarId = 1 }, new Car { CarId = 2 } };

            inventoryService.Setup(x => x.GetAvailableCars(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(cars);

            // Act
            HttpResponseMessage response =
                reservationApiController.GetAvailableCars(base._request, DateTime.Now, DateTime.Now.AddDays(1));

            Car[] data = base._GetResponseData<Car[]>(response);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreSame(cars, data);
        }

        [Test]
        public void ReserveCar()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            var expectedData = new Reservation();

            rentalService.Setup(x => x.MakeReservation(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedData);

            // Act
            HttpResponseMessage response =
                reservationApiController.ReserveCar(base._request, new ReservationModel());

            Reservation actualData = base._GetResponseData<Reservation>(response);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreSame(expectedData, actualData);
        }

        [Test]
        public void GetOpenReservations()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            CustomerReservationData[] expectedData =
            {
                new CustomerReservationData { Car = "Bmw", CustomerName = "John" }
            };

            rentalService.Setup(x => x.GetCustomerReservations(It.IsAny<string>())).Returns(expectedData);

            // Act
            HttpResponseMessage response = reservationApiController.GetOpenReservations(base._request);

            CustomerReservationData[] actualData = base._GetResponseData<CustomerReservationData[]>(response);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreSame(expectedData, actualData);
        }

        [Test]
        public void CancelReservation_sucess()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            int reservationId = 5;
            var reservation = new Reservation { ReservationId = reservationId };

            rentalService.Setup(x => x.GetReservation(reservationId)).Returns(reservation);

            // Act
            HttpResponseMessage response = reservationApiController.CancelReservation(this._request, reservationId);

            // Assert
            rentalService.Verify(x => x.GetReservation(reservationId), Times.Once);
            rentalService.Verify(x => x.CancelReservation(reservationId), Times.Once);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void CancelReservation_fail()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            // Act
            HttpResponseMessage  response = reservationApiController.CancelReservation(this._request, It.IsAny<int>());

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void GetReservationHistory()
        {
            // Arrange
            var inventoryService = new Mock<IInventoryService>();
            var rentalService = new Mock<IRentalService>();

            var reservationApiController =
                new ReservationApiController(inventoryService.Object, rentalService.Object);

            Rental[] expectedData = 
            {
                new Rental { AccountId = 1, CarId = 1 }
            };

            rentalService.Setup(x => x.GetRentalHistory(It.IsAny<string>())).Returns(expectedData);

            // Act
            HttpResponseMessage response = reservationApiController.GetReservationHistory(base._request);

            Rental[] actualData = base._GetResponseData<Rental[]>(response);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreSame(expectedData, actualData);
        }
    }
}
