using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using CarRental.Web.Core;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : ViewControllerBase
    {
        [HttpGet]
        [Route("account")]
        public ActionResult MyAccount()
        {
            return base.View();
        }

        [HttpGet]
        [Route("reserve")]
        public ActionResult ReserveCar()
        {
            return base.View();
        }

        [HttpGet]
        [Route("reservations")]
        public ActionResult CurrentReservations()
        {
            return base.View();
        }

        [HttpGet]
        [Route("history")]
        public ActionResult RentalHistory()
        {
            return base.View();
        }
    }
}