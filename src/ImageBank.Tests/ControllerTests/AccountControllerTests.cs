using System.Web.Mvc;
using ImageBank.Web.Controllers;
using ImageBank.Web.Models;
using NUnit.Framework;

namespace ImageBank.Tests.ControllerTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void Login_ShouldReturn_ViewResult()
        {
            var controller = new AccountController();

            var result = controller.Login();

            Assert.IsInstanceOf(typeof (ViewResult), result);
        }

        [Test]
        public void Login_ShouldReturn_LoginModelAsModel()
        {
            var controller = new AccountController();

            var result = controller.Login() as ViewResult;

            Assert.IsInstanceOf(typeof(LoginModel), result.Model);
        }

        [Test]
        public void Register_ShouldReturn_ViewResult()
        {
            var controller = new AccountController();

            var result = controller.Register();

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Register_ShouldReturn_RegisterModelAsModel()
        {
            var controller = new AccountController();

            var result = controller.Register() as ViewResult;

            Assert.IsInstanceOf(typeof(RegisterModel), result.Model);
        }
    }
}