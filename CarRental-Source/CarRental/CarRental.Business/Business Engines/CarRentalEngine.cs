using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Common;
using CarRental.Business.Entities;

namespace CarRental.Business
{
    public class CarRentalEngine : ICarRentalEngine
    {
        public bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate,
            IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            var isAvailable = true;

            var reservation = reservedCars.FirstOrDefault(c => c.CarId == carId);
            if (reservation != null &&
               ((pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                isAvailable = false;
            }

            if (isAvailable)
            {
                var rental = rentedCars.FirstOrDefault(c => c.CarId == carId);
                isAvailable = !(rental != null && (pickupDate <= rental.DateDue));
            }

            return isAvailable;
        }
    }
}
