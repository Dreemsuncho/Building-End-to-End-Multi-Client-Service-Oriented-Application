using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Config
{
    public class ReservationConfig : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfig()
        {
            this.HasKey(x => x.ReservationId);
        }
    }
}
