using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using NUnit.Framework;
using Moq;

using CarRental.Web.Core;
using CarRental.Web.Models;
using CarRental.Web.Controllers;

namespace CarRental.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountController_Tests
    {
        [Test]
        public void Login()
        {
            // Arrange
            var securityAdapter = new Mock<ISecurityAdapter>();

            string returnUrl = "/test/return/url";

            var accountController = new AccountController(securityAdapter.Object);

            // Act
            ActionResult result = accountController.Login(returnUrl);

            // Assert
            Assert.IsTrue(result is ViewResult);

            ViewResult viewResult = result as ViewResult;

            Assert.IsTrue(viewResult.Model is AccountLoginModel);

            AccountLoginModel model = viewResult.Model as AccountLoginModel;

            Assert.IsTrue(model.ReturnUrl == returnUrl);
        }

        [Test]
        public void Register()
        {
            // Arrange
            var securityAdapter = new Mock<ISecurityAdapter>();

            var accountController = new AccountController(securityAdapter.Object);

            // Act
            ActionResult result = accountController.Register();

            // Assert
            securityAdapter.Verify(x => x.Initialize(), Times.Once);

            Assert.IsTrue(result is ViewResult);

            ViewResult viewResult = result as ViewResult;

            Assert.IsTrue(viewResult.Model is null);
        }

        [Test]
        public void Logout()
        {
            // Arrange
            var securityAdapter = new Mock<ISecurityAdapter>();

            var accountController = new AccountController(securityAdapter.Object);

            // Act
            ActionResult result = accountController.Logout();

            // Assert
            securityAdapter.Verify(x => x.Initialize(), Times.Once);
            securityAdapter.Verify(x => x.Logout(), Times.Once);

            Assert.IsTrue(result is RedirectToRouteResult);
        }

        [Test]
        public void ChangePassword()
        {
            // Arrange
            var securityAdapter = new Mock<ISecurityAdapter>();

            var accountController = new AccountController(securityAdapter.Object);

            // Act
            ActionResult result = accountController.ChangePassword();

            Assert.IsTrue(result is ViewResult);

            ViewResult viewResult = result as ViewResult;

            Assert.IsTrue(viewResult.Model is null);
        }
    }
}
