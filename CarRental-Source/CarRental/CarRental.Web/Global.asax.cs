using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

using CarRental.Web.Core;
using CarRental.Client.Bootstrapper;

namespace CarRental.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BootstrapBundleConfig.RegisterBundles();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer container = MEFLoader.Init(catalog.Catalogs);

            // view controllers
            DependencyResolver.SetResolver(new MefDependencyResolver(container));
            // web api controllers
            GlobalConfiguration.Configuration.DependencyResolver = new MefAPIDependencyResolver(container);
        }
    }
}
