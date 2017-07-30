using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Business.Entities;

namespace CarRental.Data.Config
{
    public class CarConfig : EntityTypeConfiguration<Car>
    {
        public CarConfig()
        {
            this.HasKey(x => x.CarId);
            this.Property(x => x.Description).HasMaxLength(200);
            this.Property(x => x.Color).HasMaxLength(200);
            this.Ignore(x => x.CurrentlyRented);
        }
    }
}
