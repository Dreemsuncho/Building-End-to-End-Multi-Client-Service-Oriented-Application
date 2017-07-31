using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Security.Permissions;

using CarRental.Data.Contracts.DTOs;
using Core.Common.Exceptions;
using Core.Common.Contracts;
using CarRental.Common;
using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;


namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        [Import]
        private readonly IDataRepositoryFactory _dataRepositoryFactory;
        [Import]
        private readonly IBusinessEngineFactory _businessEngineFactory;


        #region Constructors

        public RentalManager() { }
        public RentalManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
        }
        public RentalManager(IBusinessEngineFactory businessEngineFactory)
        {
            this._businessEngineFactory = businessEngineFactory;
        }
        public RentalManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
            this._businessEngineFactory = businessEngineFactory;
        }

        #endregion


        protected override Account LoadAuthorizationValidationAccount(string loginName)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                var account = accountRepository.GetByLogin(loginName);
                if (account == null)
                {
                    var ex = new NotFoundException(
                        $"Cannot find account for login name {loginName} to use for security trimming.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return account;
            });
        }


        #region IRentalService members

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    var rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, DateTime.Now, dateDueBack);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                try
                {
                    var rental = carRentalEngine.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AcceptCarReturn(int carId)
        {
            base.ExecuteFaultHandledOperation(() =>
            {
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                var rental = rentalRepository.GetCurrentRentalByCar(carId);
                if (rental == null)
                {
                    var ex = new CarNotRentedException($"Car {carId} is not currently rented.");
                    throw new FaultException<CarNotRentedException>(ex, ex.Message);
                }

                rental.DateReturned = DateTime.Now;

                var updatedRentalEntity = rentalRepository.Update(rental);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.Car_Rental_Admin_Role)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.Car_Rental_User)]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                var account = accountRepository.GetByLogin(loginEmail);

                if (account == null)
                {
                    var ex = new NotFoundException($"No account found for login {loginEmail}.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                base.ValidateAuthorization(account);

                return rentalRepository.GetRentalHistoryByAccount(account.AccountId);
            });
        }

        public Reservation GetReservation(int reservationId)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    var ex = new NotFoundException($"No reservation found for id '{reservationId}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return reservation;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    var ex = new NotFoundException($"No account found for login '{loginEmail}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                var reservation = new Reservation
                {
                    AccountId = account.AccountId,
                    CarId = carId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                var savedEntity = reservationRepository.Add(reservation);
                return savedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();
                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                var reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    var ex = new NotFoundException($"Reservation {reservationId} is not found.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                var account = accountRepository.Get(reservation.AccountId);
                if (account == null)
                {
                    var ex = new NotFoundException($"No account found for account ID '{reservation.AccountId}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                try
                {
                    var rental = carRentalEngine.RentCarToCustomer(
                        account.LoginEmail, reservation.CarId, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (CarCurrentlyRentedException ex)
                {
                    throw new FaultException<CarCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservation);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void CancelReservation(int reservationId)
        {
            base.ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    var ex = new NotFoundException($"No reservation found found for ID '{reservationId}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservationId);
            });
        }

        public CustomerReservationData[] GetCurrentReservations()
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var reservationData = new List<CustomerReservationData>();

                var reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();
                foreach (var reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    var ex = new NotFoundException($"No account found for login '{loginEmail}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                var reservationData = new List<CustomerReservationData>();

                var reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(account.AccountId);
                foreach (var reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Car = reservationInfo.Car.Color + " " + reservationInfo.Car.Year + " " + reservationInfo.Car.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        public Rental GetRental(int rentalId)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                var rental = rentalRepository.Get(rentalId);
                if (rental == null)
                {
                    var ex = new NotFoundException($"No rental record found for id '{rentalId}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return rental;
            });
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                var rentalData = new List<CustomerRentalData>();

                var rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();
                foreach (var rentalInfo in rentalInfoSet)
                {
                    rentalData.Add(new CustomerRentalData()
                    {
                        RentalId = rentalInfo.Rental.RentalId,
                        Car = rentalInfo.Car.Color + " " + rentalInfo.Car.Year + " " + rentalInfo.Car.Description,
                        CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                        DateRented = rentalInfo.Rental.DateRented,
                        ExpectedReturn = rentalInfo.Rental.DateDue
                    });
                }

                return rentalData.ToArray();
            });
        }

        public Reservation[] GetDeadReservations()
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var reservations = reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));

                return (reservations != null ? reservations.ToArray() : null);
            });
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                return carRentalEngine.IsCarCurrentlyRented(carId);
            });
        }

        #endregion
    }
}
