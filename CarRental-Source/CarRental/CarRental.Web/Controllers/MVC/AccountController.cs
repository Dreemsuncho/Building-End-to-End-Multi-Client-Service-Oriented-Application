using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using WebMatrix.WebData;

using CarRental.Web.Core;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("account")]
    public class AccountController : ViewControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            this._securityAdapter = securityAdapter;
        }

        [HttpGet]
        [Route("register")]
        public ActionResult Register()
        {
            this._securityAdapter.Initialize();
            return base.View();
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            this._securityAdapter.Initialize();
            return base.View(new AccountLoginModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            this._securityAdapter.Initialize();
            this._securityAdapter.Logout();

            return base.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("changepassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return base.View();
        }
    }
}