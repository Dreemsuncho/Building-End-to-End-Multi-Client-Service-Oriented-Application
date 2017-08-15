using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Security.Permissions;

using Core.Common.Contracts;
using Core.Common.Exceptions;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using CarRental.Common;

namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class AccountManager : ManagerBase, IAccountService
    {
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        public AccountManager() { }
        public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            this._dataRepositoryFactory = dataRepositoryFactory;
        }


        #region IAccountService operations

        // TODO this is because running under iis express
        //[PrincipalPermission(SecurityAction.Demand, Role = Security.Car_Rental_Admin_Role)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.Car_Rental_User)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                var accountEntity = accountRepository.GetByLogin(loginEmail);
                if (accountEntity == null)
                {
                    var ex = new NotFoundException($"Account with login {loginEmail} is not in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                base.ValidateAuthorization(accountEntity);

                return accountEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        // TODO this is because running under iis express
        //[PrincipalPermission(SecurityAction.Demand, Role = Security.Car_Rental_Admin_Role)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.Car_Rental_User)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            base.ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = this._dataRepositoryFactory.GetDataRepository<IAccountRepository>();

                base.ValidateAuthorization(account);

                accountRepository.Update(account);
            });
        }

        #endregion
    }
}
