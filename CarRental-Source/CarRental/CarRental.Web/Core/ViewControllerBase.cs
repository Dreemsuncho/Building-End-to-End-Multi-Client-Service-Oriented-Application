using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Web.Core
{
    public class ViewControllerBase : Controller
    {
        private List<IServiceContract> _disposableServices = new List<IServiceContract>();

        protected virtual void RegisterServices(List<IServiceContract> disposableServices)
        {
        }

        protected List<IServiceContract> DisposableServices
        {
            get
            {
                return _disposableServices;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.RegisterServices(this.DisposableServices);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            foreach (var service in this.DisposableServices)
            {
                if (service != null &&
                    service is IDisposable)
                {
                    (service as IDisposable).Dispose();
                }
            }
        }
    }
}