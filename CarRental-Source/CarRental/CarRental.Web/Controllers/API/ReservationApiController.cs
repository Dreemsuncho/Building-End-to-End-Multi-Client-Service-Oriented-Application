using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Core.Common.Contracts;
using CarRental.Web.Core;
using CarRental.Client.Contracts;
using CarRental.Client.Entities;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [UsesDisposableService]
    [RoutePrefix("api/reservation")]
    public class ReservationApiController : ApiControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IRentalService _rentalService;

        [ImportingConstructor]
        public ReservationApiController(IInventoryService inventoryService, IRentalService rentalService)
        {
            this._inventoryService = inventoryService;
            this._rentalService = rentalService;
        }

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(this._inventoryService);
            disposableServices.Add(this._rentalService);
        }

        [HttpGet]
        [Route("availablecars")]
        public HttpResponseMessage GetAvailableCars(HttpRequestMessage request, DateTime pickupDate, DateTime returnDate)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                Car[] cars = this._inventoryService.GetAvailableCars(pickupDate, returnDate);

                response = request.CreateResponse(HttpStatusCode.OK, cars);

                return response;
            });
        }

        [HttpPost]
        [Route("reservecar")]
        public HttpResponseMessage ReserveCar(HttpRequestMessage request, [FromBody]ReservationModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                string user = base.User.Identity.Name;
                Reservation reservation = this._rentalService.MakeReservation(user, model.carId, model.pickupDate, model.returnDate);

                response = request.CreateResponse(HttpStatusCode.OK, reservation);

                return response;
            });
        }

        [HttpGet]
        [Route("getopen")]
        public HttpResponseMessage GetOpenReservations(HttpRequestMessage request)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                string user = base.User.Identity.Name;
                CustomerReservationData[] reservations = this._rentalService.GetCustomerReservations(user);

                response = request.CreateResponse(HttpStatusCode.OK, reservations);

                return response;
            });
        }

        [HttpPost]
        [Route("cancel")]
        public HttpResponseMessage CancelReservation(HttpRequestMessage request, [FromBody]int reservationId)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                Reservation reservation = this._rentalService.GetReservation(reservationId);

                if (reservation != null)
                {
                    this._rentalService.CancelReservation(reservationId);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound, "No reservation found with id '" + reservationId + "'");
                }

                return response;
            });
        }

        [HttpGet]
        [Route("history")]
        public HttpResponseMessage GetReservationHistory(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;

            string user = base.User.Identity.Name;

            Rental[] rentalHistory = this._rentalService.GetRentalHistory(user);

            response = request.CreateResponse(HttpStatusCode.OK, rentalHistory);

            return response;
        }
    }
}
