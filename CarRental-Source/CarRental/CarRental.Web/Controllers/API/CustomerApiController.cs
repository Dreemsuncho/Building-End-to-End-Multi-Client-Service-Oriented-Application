using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using System.Text.RegularExpressions;

using Core.Common.Contracts;
using CarRental.Client.Contracts;
using CarRental.Client.Entities;
using CarRental.Web.Core;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [UsesDisposableService]
    [RoutePrefix("api/customer")]
    public class CustomerApiController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        [ImportingConstructor]
        public CustomerApiController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(this._accountService);
        }

        [HttpGet]
        [Route("account")]
        public HttpResponseMessage GetCustomerAccountInfo(HttpRequestMessage request)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                Account account = this._accountService.GetCustomerAccountInfo(base.User.Identity.Name);

                response = request.CreateResponse(HttpStatusCode.OK, account);

                return response;
            });
        }

        [HttpPost]
        [Route("account")]
        public HttpResponseMessage SaveCustomerAccountInfo(HttpRequestMessage request, Account model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var errors = new List<string>();
                base.ValidateAuthorizedUser(model.LoginEmail);

                var states = UIHelper.GetStates();

                var state = states.FirstOrDefault(s => s.Abbrev.ToUpper() == model.State.ToUpper());
                if (state == null)
                    errors.Add("Invalid state.");

                var matchZipCode = Regex.Match(model.ZipCode, @"^\d{5}(?:[-\s]\d{4})?$");
                if (!matchZipCode.Success)
                    errors.Add("Zip code is an invalid format.");

                if (model.CreditCard.Length != 16)
                    errors.Add("Credit card number is in an invalid format.");

                var matchExpDate = Regex.Match(model.ExpDate, @"(0[1-9]|1[0-2])\/[0-9]{2}", RegexOptions.IgnoreCase);
                if (!matchExpDate.Success)
                    errors.Add("Expiration date is invalid.");

                if (errors.Count == 0)
                {
                    // trim out the / in the exp date
                    model.ExpDate = model.ExpDate.Replace("/", "");
                    this._accountService.UpdateCustomerAccountInfo(model);

                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
                }

                return response;
            });
        }
    }
}
