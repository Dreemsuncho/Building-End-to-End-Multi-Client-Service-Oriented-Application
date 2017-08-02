using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Core.Common.ServiceModel;
using CarRental.Client.Contracts;
using CarRental.Client.Entities;

namespace CarRental.Client.Proxies
{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : UserClientBase<IRentalService>, IRentalService
    {
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return base.Channel.RentCarToCustomer(loginEmail, carId, dateDueBack);
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return base.Channel.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
        }

        public void AcceptCarReturn(int carId)
        {
            base.Channel.AcceptCarReturn(carId);
        }

        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return base.Channel.GetRentalHistory(loginEmail);
        }

        public Reservation GetReservation(int reservationId)
        {
            return base.Channel.GetReservation(reservationId);
        }

        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return base.Channel.MakeReservation(loginEmail, carId, rentalDate, returnDate);
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            base.Channel.ExecuteRentalFromReservation(reservationId);
        }

        public void CancelReservation(int reservationId)
        {
            base.Channel.CancelReservation(reservationId);
        }

        public CustomerReservationData[] GetCurrentReservations()
        {
            return base.Channel.GetCurrentReservations();
        }

        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return base.Channel.GetCustomerReservations(loginEmail);
        }

        public Rental GetRental(int rentalId)
        {
            return base.Channel.GetRental(rentalId);
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            return base.Channel.GetCurrentRentals();
        }

        public Reservation[] GetDeadReservations()
        {
            return base.Channel.GetDeadReservations();
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return base.Channel.IsCarCurrentlyRented(carId);
        }


        #region Async operations

        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carId, DateTime dateDueBack)
        {
            return base.Channel.RentCarToCustomerAsync(loginEmail, carId, dateDueBack);
        }

        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return base.Channel.RentCarToCustomerAsync(loginEmail, carId, rentalDate, dateDueBack);
        }

        public Task AcceptCarReturnAsync(int carId)
        {
            return base.Channel.AcceptCarReturnAsync(carId);
        }

        public Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail)
        {
            return base.Channel.GetRentalHistoryAsync(loginEmail);
        }

        public Task<Reservation> GetReservationAsync(int reservationId)
        {
            return base.Channel.GetReservationAsync(reservationId);
        }

        public Task<Reservation> MakeReservationAsync(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return base.Channel.MakeReservationAsync(loginEmail, carId, rentalDate, returnDate);
        }

        public Task ExecuteRentalFromReservationAsync(int reservationId)
        {
            return base.Channel.ExecuteRentalFromReservationAsync(reservationId);
        }

        public Task CancelReservationAsync(int reservationId)
        {
            return base.Channel.CancelReservationAsync(reservationId);
        }

        public Task<CustomerReservationData[]> GetCurrentReservationsAsync()
        {
            return base.Channel.GetCurrentReservationsAsync();
        }

        public Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail)
        {
            return base.Channel.GetCustomerReservationsAsync(loginEmail);
        }

        public Task<Rental> GetRentalAsync(int rentalId)
        {
            return base.Channel.GetRentalAsync(rentalId);
        }

        public Task<CustomerRentalData[]> GetCurrentRentalsAsync()
        {
            return base.Channel.GetCurrentRentalsAsync();
        }

        public Task<Reservation[]> GetDeadReservationsAsync()
        {
            return base.Channel.GetDeadReservationsAsync();
        }

        public Task<bool> IsCarCurrentlyRentedAsync(int carId)
        {
            return base.Channel.IsCarCurrentlyRentedAsync(carId);
        }

        #endregion
    }
}
