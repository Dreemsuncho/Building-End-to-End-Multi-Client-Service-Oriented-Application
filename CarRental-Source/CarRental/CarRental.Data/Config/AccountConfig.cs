using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Config
{
    public class AccountConfig : EntityTypeConfiguration<Account>
    {
        public AccountConfig()
        {
            this.HasKey(x => x.AccountId);
            this.Property(x => x.LoginEmail).HasMaxLength(50);
            this.Property(x => x.FirstName).HasMaxLength(50);
            this.Property(x => x.LastName).HasMaxLength(50);
            this.Property(x => x.Address).HasMaxLength(50);
            this.Property(x => x.City).HasMaxLength(50);
            this.Property(x => x.State).HasMaxLength(50);
            this.Property(x => x.ZipCode).HasMaxLength(50);
            this.Property(x => x.CreditCard).HasMaxLength(50);
            this.Property(x => x.ExpDate).HasMaxLength(50);
        }
    }
}
