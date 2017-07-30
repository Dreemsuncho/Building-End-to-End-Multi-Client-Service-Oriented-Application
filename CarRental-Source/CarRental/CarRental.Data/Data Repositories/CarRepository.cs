using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using System.ComponentModel.Composition;

namespace CarRental.Data
{
    [Export(typeof(ICarRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
        protected override Car AddEntity(CarRentalContext entityContext, Car entity)
        {
            return entityContext.CarSet.Add(entity);
        }

        protected override IEnumerable<Car> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.CarSet.ToList();
        }

        protected override Car GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.CarSet.Find(id);
        }

        protected override Car UpdateEntity(CarRentalContext entityContext, Car entity)
        {
            return entityContext.CarSet
                .FirstOrDefault(e => e.CarId == entity.CarId);
        }
    }
}
