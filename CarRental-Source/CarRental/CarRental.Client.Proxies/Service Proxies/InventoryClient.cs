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
    [Export(typeof(IInventoryService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryClient : UserClientBase<IInventoryService>, IInventoryService
    {
        public void DeleteCar(int carId)
        {
            base.InvokeSecurityWrappedMethod(() => base.Channel.DeleteCar(carId));
        }

        public Car[] GetAllCars()
        {
            return base.InvokeSecurityWrappedMethod(() => base.Channel.GetAllCars());
        }

        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return base.InvokeSecurityWrappedMethod(() => base.Channel.GetAvailableCars(pickupDate, returnDate));
        }

        public Car GetCar(int carId)
        {
            return base.InvokeSecurityWrappedMethod(() => base.Channel.GetCar(carId));
        }

        public Car UpdateCar(Car car)
        {
            return base.InvokeSecurityWrappedMethod(() => base.Channel.UpdateCar(car));
        }


        #region Async

        public Task DeleteCarAsync(int carId)
        {
            return base.Channel.DeleteCarAsync(carId);
        }

        public Task<Car[]> GetAllCarsAsync()
        {
            return base.Channel.GetAllCarsAsync();
        }

        public Task<Car[]> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate)
        {
            return base.Channel.GetAvailableCarsAsync(pickupDate, returnDate);
        }

        public Task<Car> GetCarAsync(int carId)
        {
            return base.Channel.GetCarAsync(carId);
        }

        public Task<Car> UpdateCarAsync(Car car)
        {
            return base.Channel.UpdateCarAsync(car);
        }

        #endregion
    }
}
