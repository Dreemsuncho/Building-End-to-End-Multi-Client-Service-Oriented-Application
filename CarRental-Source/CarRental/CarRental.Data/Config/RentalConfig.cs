using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Config
{
    class RentalConfig : EntityTypeConfiguration<Rental>
    {
        public RentalConfig()
        {
            this.HasKey(x => x.RentalId);
        }
    }
}
