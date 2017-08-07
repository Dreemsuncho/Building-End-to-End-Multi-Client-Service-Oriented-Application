using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Http.Dependencies;

using Core.Common.Extensions;

namespace CarRental.Web.Core
{
    public class MefAPIDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefAPIDependencyResolver(CompositionContainer container)
        {
            this._container = container;
        }


        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return this._container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetExportedValuesByType(serviceType);
        }

        public void Dispose() { }
    }
}
