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
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        protected override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.ReservationSet.ToList();
        }

        protected override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.ReservationSet.Find(id);
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet
                .FirstOrDefault(e => e.ReservationId == entity.ReservationId);
        }


        #region IReservationRepository members

        public IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate)
        {
            using (var context = new CarRentalContext())
            {
                return context.ReservationSet
                    .Where(e => e.RentalDate < pickupDate)
                    .ToList();
            }
        }

        #endregion
    }
}
