using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ComponentModel.Composition;

using Core.Common.ServiceModel;
using CarRental.Client.Contracts;
using CarRental.Client.Entities;

namespace CarRental.Client.Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class AccountClient : UserClientBase<IAccountService>, IAccountService
    {
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return base.Channel.GetCustomerAccountInfo(loginEmail);
        }

        public void UpdateCustomerAccountInfo(Account account)
        {
            base.Channel.UpdateCustomerAccountInfo(account);
        }


        #region Async

        public Task<Account> GetCustomerAccountInfoAsync(string loginEmail)
        {
            return base.Channel.GetCustomerAccountInfoAsync(loginEmail);
        }

        public Task UpdateCustomerAccountInfoAsync(Account account)
        {
            return base.Channel.UpdateCustomerAccountInfoAsync(account);
        }

        #endregion
    }
}
