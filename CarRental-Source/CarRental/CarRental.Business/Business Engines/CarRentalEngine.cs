using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Core.Common.Exceptions;
using Core.Common.Contracts;
using CarRental.Data.Contracts;
using CarRental.Business.Common;
using CarRental.Business.Entities;

namespace CarRental.Business
{
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine : ICarRentalEngine
    {
        private IDataRepositoryFactory _dataRepositoryFactory;

        [ImportingConstructor]
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
        }


        #region ICarRentalEngine members

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

        public bool IsCarCurrentlyRented(int carId)
        {
            var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            return currentRental != null;
        }

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            return currentRental != null && currentRental.AccountId == accountId;
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException($"Cannot rent for date {rentalDate.ToShortDateString()} yet.");

            var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

            bool carIsRented = this.IsCarCurrentlyRented(carId);
            if (carIsRented)
                throw new CarCurrentlyRentedException($"Car {carId} is already rented.");

            var account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
                throw new NotFoundException($"No account found for login '{loginEmail}'.");

            var rental = new Rental
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            return rentalRepository.Add(rental);
        }

        #endregion
    }
}
