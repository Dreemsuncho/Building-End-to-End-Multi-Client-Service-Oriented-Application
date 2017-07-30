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
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {
        protected override Account AddEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.AccountSet.ToList();
        }

        protected override Account GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.AccountSet.Find(id);
        }

        protected override Account UpdateEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet
                .FirstOrDefault(e => e.AccountId == entity.AccountId);
        }


        #region IAccountRepository members

        public Account GetByLogin(string login)
        {
            using (var context = new CarRentalContext())
            {
                return context.AccountSet
                    .FirstOrDefault(e => e.LoginEmail == login);
            }
        }

        #endregion
    }
}
