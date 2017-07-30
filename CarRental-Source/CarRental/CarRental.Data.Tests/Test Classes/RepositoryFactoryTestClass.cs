using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Contracts;
using Core.Common.Core;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;

namespace CarRental.Data.Tests
{
    class RepositoryFactoryTestClass
    {
        [Import]
        private IDataRepositoryFactory _repositoryFactory;

        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryFactoryTestClass(IDataRepositoryFactory repositoryFactory)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Car> GetCars()
        {
            var carRepository = this._repositoryFactory.GetDataRepository<ICarRepository>();

            var cars = carRepository.Get();

            return cars;
        }
    }
}
