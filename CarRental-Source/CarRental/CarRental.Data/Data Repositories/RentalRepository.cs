using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Data.Contracts;

namespace CarRental.Data
{
    public class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {
        protected override Rental AddEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override IEnumerable<Rental> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.RentalSet.ToList();
        }

        protected override Rental GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.RentalSet.Find(id);
        }

        protected override Rental UpdateEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet
                .FirstOrDefault(e => e.RentalId == entity.RentalId);
        }


        #region IRentalRepository members

        public IEnumerable<Rental> GetCurrentlyRentedCars()
        {
            using (var context = new CarRentalContext())
            {
                return context.RentalSet
                    .Where(e => e.DateRented == null)
                    .ToList();
            }
        }

        public Rental GetCurrentRentalByCar(int carId)
        {
            using (var context = new CarRentalContext())
            {
                return context.RentalSet
                    .FirstOrDefault(e => e.CarId == carId && e.DateRented == null);
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (var context = new CarRentalContext())
            {
                return context.RentalSet
                    .Where(e => e.AccountId == accountId)
                    .ToList();
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByCar(int carId)
        {
           using(var context = new CarRentalContext())
            {
                return context.RentalSet
                    .Where(e => e.CarId == carId)
                    .ToList();
            }
        }

        #endregion
    }
}
