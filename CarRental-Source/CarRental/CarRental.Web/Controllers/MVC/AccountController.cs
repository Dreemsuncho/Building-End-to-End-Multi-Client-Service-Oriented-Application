using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using AttributeRouting.Web.Mvc;
using WebMatrix.WebData;

using CarRental.Web.Core;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountController : ViewControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            this._securityAdapter = securityAdapter;
        }

        [HttpGet]
        [GET("account/register")]
        public ActionResult Register()
        {
            this._securityAdapter.Initialize();
            return base.View();
        }

        [HttpGet]
        [GET("account/login")]
        public ActionResult Login(string returnUrl)
        {
            this._securityAdapter.Initialize();
            return base.View(new AccountLoginModel { ReturnUrl = returnUrl });
        }

        [HttpGet]
        [GET("account/logout")]
        public ActionResult Logout()
        {
            this._securityAdapter.Initialize();
            WebSecurity.Logout();
            return base.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [GET("account/changepassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return base.View();
        }
    }
}