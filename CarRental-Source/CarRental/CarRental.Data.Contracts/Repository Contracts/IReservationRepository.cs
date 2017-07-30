using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Contracts;
using CarRental.Business.Entities;

namespace CarRental.Data.Contracts
{
    public interface IReservationRepository : IDatRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate);
    }
}
