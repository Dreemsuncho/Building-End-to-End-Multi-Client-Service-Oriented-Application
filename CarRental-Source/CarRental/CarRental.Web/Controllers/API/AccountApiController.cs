﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;


using CarRental.Web.Core;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    public class AccountApiController : ApiControllerBase
    {
        private readonly ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountApiController(ISecurityAdapter securityAdapter)
        {
            this._securityAdapter = securityAdapter;
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody]AccountLoginModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var sucess = this._securityAdapter.Login(model.LoginEmail, model.Password, model.RememberMe);

                if (sucess)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized login attempt.");

                return response;
            });
        }


        [HttpPost]
        [Route("register/step1")]
        public HttpResponseMessage RegisterStep1(HttpRequestMessage request, [FromBody]AccountRegisterModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var errors = new List<string>();
                var states = UIHelper.GetStates();

                var state = states.FirstOrDefault(s => s.Abbrev.ToUpper() == model.State.ToUpper());
                if (state == null)
                    errors.Add("Invalid state.");

                var matchZipCode = Regex.Match(model.ZipCode, @"^\d{5}(?:[-\s]\d{4})?$");
                if (!matchZipCode.Success)
                    errors.Add("Zip code is an invalid format.");

                if (errors.Count == 0)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }

        [HttpPost]
        [Route("register/step2")]
        public HttpResponseMessage RegisterStep2(HttpRequestMessage request, [FromBody]AccountRegisterModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                bool userExist = this._securityAdapter.UserExists(model.LoginEmail);
                if (!userExist)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        new[] { "An account is already registered with this email address." });

                return response;
            });
        }

        [HttpPost]
        [Route("register/step3")]
        public HttpResponseMessage RegisterStep3(HttpRequestMessage request, [FromBody]AccountRegisterModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var errors = new List<string>();

                if (model.CreditCard.Length != 16)
                    errors.Add("Credit card number is in an invalid format.");

                var datePattern = @"^(0[1-9]|1[0-2])\/(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/[0-9]{4}$";
                var matchExpDate = Regex.Match(model.ExpDate, datePattern, RegexOptions.IgnoreCase);
                if (!matchExpDate.Success)
                    errors.Add("Expiration date is invalid.");

                if (errors.Count == 0)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());

                return response;
            });
        }

        [HttpPost]
        [Route("register")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request, [FromBody]AccountRegisterModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (this.RegisterStep1(request, model).IsSuccessStatusCode &&
                    this.RegisterStep2(request, model).IsSuccessStatusCode &&
                    this.RegisterStep3(request, model).IsSuccessStatusCode)
                {
                    this._securityAdapter.Register(model.LoginEmail, model.Password,
                        new
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Address = model.Address,
                            City = model.City,
                            State = model.State,
                            ZipCode = model.ZipCode,
                            CreditCard = model.CreditCard,
                            ExpDate = model.ExpDate.Replace("/", "")
                        });

                    this._securityAdapter.Login(model.LoginEmail, model.Password, false);

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("changepw")]
        public HttpResponseMessage ChangePassword(HttpRequestMessage request, [FromBody]AccountChangePasswordModel model)
        {
            return base.GetHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                base.ValidateAuthorizedUser(model.LoginEmail);

                bool success = this._securityAdapter.ChangePassword(model.LoginEmail, model.OldPassword, model.NewPassword);

                if (success)
                    response = request.CreateResponse(HttpStatusCode.OK);
                else
                    response = request.CreateResponse(HttpStatusCode.InternalServerError, "Unable to change password");

                return response;
            });
        }
    }
}
