using System.Collections.Generic;
using System.Web.Mvc;
using ImageBank.Core;
using ImageBank.Services.Image;
using ImageBank.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void IndexGet_ShouldReturn_ViewResult()
        {
            var controller = GetHomeController();

            var result = controller.Index();

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void IndexGet_ShouldReturn_ImagesAsModel()
        {
            var controller = GetHomeController();

            var result = controller.Index() as ViewResult;

            Assert.IsInstanceOf(typeof(IEnumerable<Image>), result.Model);
        }

        private HomeController GetHomeController()
        {
            var mockImageService = new Mock<IImageService>();
            var controller = new HomeController(mockImageService.Object);

            return controller;
        }
    }
}
