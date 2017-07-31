using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using CarRental.Data.Contracts.DTOs;

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

        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (var context = new CarRentalContext())
            {
                return context.ReservationSet
                    .Join(context.AccountSet, r => r.AccountId, a => a.AccountId, (r, a) => new { r, a })
                    .Join(context.CarSet, ra => ra.r.CarId, c => c.CarId, (ra, c) => new CustomerReservationInfo
                                                                                     {
                                                                                         Customer = ra.a,
                                                                                         Reservation = ra.r,
                                                                                         Car = c
                                                                                     })
                    .ToList();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            using (var context = new CarRentalContext())
            {
                return context.ReservationSet
                    .Join(context.AccountSet, r => r.AccountId, a => a.AccountId, (r, a) => new { r, a })
                    .Join(context.CarSet, ra => ra.r.CarId, c => c.CarId, (ra,c)=> new CustomerReservationInfo
                                                                                   {
                                                                                       Customer = ra.a,
                                                                                       Reservation = ra.r,
                                                                                       Car = c
                                                                                   })
                   .Where(x=>x.Reservation.AccountId == accountId)
                   .ToList();
            }
        }

        #endregion
    }
}
