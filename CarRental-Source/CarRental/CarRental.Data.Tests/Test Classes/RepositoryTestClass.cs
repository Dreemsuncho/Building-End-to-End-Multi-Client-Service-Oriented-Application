using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Core.Common.Core;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;

namespace CarRental.Data.Tests
{
    class RepositoryTestClass
    {
        [Import]
        private ICarRepository _carRepository;

        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryTestClass(ICarRepository carRepository)
        {
            this._carRepository = carRepository;
        }

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = this._carRepository.Get();
            return cars;
        }
    }
}
