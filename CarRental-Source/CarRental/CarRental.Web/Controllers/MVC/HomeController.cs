using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using CarRental.Web.Core;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : ViewControllerBase
    {
        public ActionResult Index()
        {
            return base.View();
        }
    }
}