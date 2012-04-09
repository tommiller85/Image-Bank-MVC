using System.Web.Mvc;
using ImageBank.Services.Account;
using ImageBank.Web.Controllers;
using ImageBank.Web.Models;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ControllerTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void LoginGet_ShouldReturn_ViewResult()
        {
            var controller = GetAccountController();

            var result = controller.Login();

            Assert.IsInstanceOf(typeof (ViewResult), result);
        }

        [Test]
        public void LoginGet_ShouldReturn_LoginModelAsModel()
        {
            var controller = GetAccountController();

            var result = controller.Login() as ViewResult;

            Assert.IsInstanceOf(typeof(LoginModel), result.Model);
        }

        [Test]
        public void LoginPost_WhenCredentialsAreIncorrect_ShouldReturnViewResult()
        {           
            var controller = GetAccountController();

            var result = controller.Login(new LoginModel { Username = "testuser", Password="incorrectpassword" });

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void LoginPost_WhenCredentialsAreIncorrect_ShouldReturnError()
        {
            var controller = GetAccountController();

            var result = controller.Login(new LoginModel { Username = "testuser", Password = "incorrectpassword" }) as ViewResult;

            Assert.AreEqual("Bad username or password combination.", controller.ModelState[""].Errors[0].ErrorMessage);
        }

        [Test]
        public void LoginPost_WhenCredentialsAreCorrect_ShouldRedirectHome()
        {
            var controller = GetAccountController();

            var result = controller.Login(new LoginModel { Username = "testuser", Password = "correctpassword" }) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }

        [Test]
        public void LoginPost_WhenCredentialsAreCorrectAndReturnUrlIsNotNull_ShouldRedirectToReturnUrl()
        {
            var controller = GetAccountController();

            var result = controller.Login(new LoginModel { Username = "testuser", Password = "correctpassword" }, "/Image/Upload") as RedirectResult;

            Assert.AreEqual("/Image/Upload", result.Url);
        }

        [Test]
        public void LoginPost_WhenCredentialsAreCorrect_ShouldSetAuthCookie()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.Authenticate("testuser", "correctpassword")).Returns(true);
            mockAccountService.Setup(x => x.SetAuthCookie("testuser", false)).Verifiable();

            var controller = new AccountController(mockAccountService.Object);

            var result = controller.Login(new LoginModel { Username = "testuser", Password = "correctpassword" });

            mockAccountService.Verify(x => x.SetAuthCookie("testuser", false));
        }

        [Test]
        public void RegisterGet_ShouldReturn_ViewResult()
        {
            var controller = GetAccountController();

            var result = controller.Register();

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void RegisterGet_ShouldReturn_RegisterModelAsModel()
        {
            var controller = GetAccountController();

            var result = controller.Register() as ViewResult;

            Assert.IsInstanceOf(typeof(RegisterModel), result.Model);
        }

        private AccountController GetAccountController()
        {
            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(x => x.Authenticate("testuser", "correctpassword")).Returns(true);
            mockAccountService.Setup(x => x.Authenticate("testuser", "incorrectpassword")).Returns(false);

            var controller = new AccountController(mockAccountService.Object);

            return controller;
        }
    }
}