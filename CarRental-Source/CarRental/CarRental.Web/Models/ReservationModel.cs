using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Web.Models
{
    public class ReservationModel
    {
        public int carId { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime returnDate { get; set; }
    }
}