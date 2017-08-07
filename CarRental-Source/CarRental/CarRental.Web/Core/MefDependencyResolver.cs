using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Mvc;

using Core.Common.Extensions;

namespace CarRental.Web.Core
{
    public class MefDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container)
        {
            this._container = container;
        }


        public object GetService(Type serviceType)
        {
            return this._container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetExportedValuesByType(serviceType);
        }
    }
}
