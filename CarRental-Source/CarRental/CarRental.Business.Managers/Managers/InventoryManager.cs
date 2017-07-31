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
        private readonly IDataRepositoryFactory _repositoryFactory;

        public InventoryManager() { }
        public InventoryManager(IDataRepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }


        #region IInventoryService members

        public Car GetCar(int carId)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var dataRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();

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
                var dataRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = this._repositoryFactory.GetDataRepository<IRentalRepository>();

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
                var carRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();

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
                var carRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();
                carRepository.Remove(carId);
            });
        }

        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var carRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = this._repositoryFactory.GetDataRepository<IRentalRepository>();
                var reservationRepository = this._repositoryFactory.GetDataRepository<IReservationRepository>();

                var allCars = carRepository.Get();
                var rentedCars = rentalRepository.GetCurrentlyRentedCars();
                var reservedCars = reservationRepository.Get();

                var availableCars = new List<Car>();

                foreach (var car in allCars)
                {

                }

                return availableCars.ToArray();
            });
        }

        #endregion
    }
}
