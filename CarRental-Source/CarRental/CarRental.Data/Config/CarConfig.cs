using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Config
{
    class CarConfig : EntityTypeConfiguration<Car>
    {
        public CarConfig()
        {
            this.HasKey(x => x.CarId);
        }
    }
}
