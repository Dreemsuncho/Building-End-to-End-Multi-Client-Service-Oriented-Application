using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;

using Core.Common.Contracts;
using Core.Common.Exceptions;
using Core.Common.Core;
using CarRental.Common;
using CarRental.Business.Entities;

namespace CarRental.Business.Managers
{
    public abstract class ManagerBase
    {
        protected string loginName;
        protected Account authorizationAccount;

        public ManagerBase()
        {
            var context = OperationContext.Current;
            if (context != null)
            {
                this.loginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");
                // because maybe it's desktop user
                if (this.loginName.IndexOf(@"\") > -1)
                    this.loginName = string.Empty;
            }

            ObjectBase.Container?.SatisfyImportsOnce(this);

            if (!string.IsNullOrWhiteSpace(this.loginName))
            {
                this.authorizationAccount =
                    this.LoadAuthorizationValidationAccount(this.loginName);
            }
        }

        protected virtual Account LoadAuthorizationValidationAccount(string loginName)
        {
            return null;
        }

        protected void ValidateAuthorization(IAccountOwnedEntity entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.Car_Rental_Admin_Role))
            {
                if (this.authorizationAccount != null)
                {
                    if (this.loginName != string.Empty &&
                        entity.OwnerAccountId != this.authorizationAccount.AccountId)
                    {
                        var ex = new AuthorizationValidationException(
                            "Attempt to access a secure record with improper user authorization validation.");
                        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                    }
                }
            }
        }

        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
