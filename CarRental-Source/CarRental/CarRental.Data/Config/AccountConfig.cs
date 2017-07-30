using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Config
{
    class AccountConfig : EntityTypeConfiguration<Account>
    {
        public AccountConfig()
        {
            this.HasKey(x => x.AccountId);
        }
    }
}
