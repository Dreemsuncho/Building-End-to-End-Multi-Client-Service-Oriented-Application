using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using Core.Common.Core;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using CarRental.Business.Entities;
using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Data.Contracts;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        [Import]
        private readonly IDataRepositoryFactory _dataRepositoryFactory;
        [Import]
        private readonly IBusinessEngineFactory _businessEngineFactory;


        public InventoryManager() { }
        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
        }
        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            this._businessEngineFactory = businessEngineFactory;
        }
        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory,
                                IBusinessEngineFactory businessEngineFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
            this._businessEngineFactory = businessEngineFactory;
        }

        #region IInventoryService members

        public Car GetCar(int carId)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var dataRepository = this._dataRepositoryFactory.GetDataRepository<ICarRepository>();

                var car = dataRepository.Get(carId);

                if (car == null)
                {
                    var ex = new NotFoundException($"Car with ID of {carId} is not in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return car;
            });
        }

        public Car[] GetAllCars()
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var dataRepository = this._dataRepositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                var cars = dataRepository.Get();
                var rentedCars = rentalRepository.GetCurrentlyRentedCars();

                foreach (var car in cars)
                {
                    var rentedCar = rentedCars.FirstOrDefault(c => c.CarId == car.CarId);
                    car.CurrentlyRented = (rentedCar != null);
                }

                return cars.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Car UpdateCar(Car car)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRepository = this._dataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car updatedCar;

                if (car.CarId == 0)
                    updatedCar = carRepository.Add(car);
                else
                    updatedCar = carRepository.Update(car);

                return updatedCar;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteCar(int carId)
        {
            base.ExecuteFaultHandledOperation(() =>
            {
                var carRepository = this._dataRepositoryFactory.GetDataRepository<ICarRepository>();
                carRepository.Remove(carId);
            });
        }

        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRepository = this._dataRepositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = this._dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var reservationRepository = this._dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var carRentalEngine = this._businessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                var allCars = carRepository.Get();
                var rentedCars = rentalRepository.GetCurrentlyRentedCars();
                var reservedCars = reservationRepository.Get();

                var availableCars = new List<Car>();

                foreach (var car in allCars)
                {
                    var isAvailable = carRentalEngine.IsCarAvailableForRental(
                        car.CarId, pickupDate, returnDate, rentedCars, reservedCars);

                    if (isAvailable)
                    {
                        availableCars.Add(car);
                    }
                }

                return availableCars.ToArray();
            });
        }

        #endregion
    }
}
