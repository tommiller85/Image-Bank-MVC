using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageBank.Core;
using ImageBank.Persistence;
using ImageBank.Services.Image;
using ImageBank.Services.ImageProcessing;
using ImageBank.Web.Controllers;
using ImageBank.Web.Models;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ControllerTests
{
    [TestFixture]
    public class ImageControllerTests
    {
        [Test]
        public void UploadGet_ShouldReturn_ViewResult()
        {
            var controller = GetImageController();

            var result = controller.Upload();

            Assert.IsInstanceOf(typeof (ViewResult), result);
        }

        [Test]
        public void UploadPost_ShouldReturn_ContentResult()
        {
            var controller = GetImageController();

            var result = controller.Upload(0, 0, "foo.jpg");

            Assert.IsInstanceOf(typeof (ContentResult), result);
        }

        [Test]
        public void UploadPost_ShouldInvoke_ProcessImageChunk()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(x => x.ProcessImageChunk(new ImageChunk(), "testuser")).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(0, 0, "foo.jpg");

            mockImageProcessor.Verify(x => x.ProcessImageChunk(It.IsAny<ImageChunk>(), It.IsAny<string>()));
        }

        [Test]
        public void EditImagesGet_ShouldReturn_ViewResult()
        {
            var controller = GetImageController();

            var result = controller.EditImages();

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void EditImagesGet_ShouldReturn_EditImageModelsAsModel()
        {
            var controller = GetImageController();

            var result = controller.EditImages() as ViewResult;

            Assert.IsInstanceOf(typeof(IEnumerable<EditImageModel>), result.Model);
        }

        [Test]
        public void EditImagesGet_ShouldReturn_PagingInfoAsViewData()
        {
            var controller = GetImageController();

            var result = controller.EditImages() as ViewResult;

            Assert.AreEqual(0, controller.ViewData["PageIndex"]);
            Assert.AreEqual(false, controller.ViewData["HasPrevious"]);
            Assert.AreEqual(false, controller.ViewData["HasNext"]);
        }

        [Test]
        public void EditImagesPost_ShouldReturn_RedirectToRouteResult()
        {
            var controller = GetImageController();

            var result = controller.EditImages(new List<EditImageModel>()) as RedirectToRouteResult;

            Assert.AreEqual("EditImages", result.RouteValues["action"]);
            Assert.AreEqual("Image", result.RouteValues["controller"]);
            Assert.AreEqual(0, result.RouteValues["pageIndex"]);
        }

        [Test]
        public void EditImagesPost_ShouldInvoke_EditImages()
        {
            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.EditImages(new List<Image>()));
            var controller = GetImageController(imageService: mockImageService.Object);

            var result = controller.EditImages(new List<EditImageModel>());

            mockImageService.Verify(x => x.EditImages(It.IsAny<IEnumerable<Image>>()), Times.Once());
        }

        private ImageController GetImageController(IImageProcessor imageProcessor = null, IImageService imageService = null)
        {
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            var postedfile = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();

            var mockImageProcessor = new Mock<IImageProcessor>();
            var mockSettingRepository = new Mock<ISettingRepository>();
            var mockImageService = new Mock<IImageService>();

            mockImageService.Setup(x => x.GetImagesByUser("testuser", 0, 5)).Returns(
                new PagedList<Image>(new List<Image>().AsQueryable(), 0, 5));

            var fakeFileKeys = new List<string> {"file"};

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Server.MapPath("~/Images/Upload/Original")).Returns("C:\\Images\\Upload\\Original");
            context.SetupGet(ctx => ctx.User.Identity.Name).Returns("testuser");

            request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);

            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys[0]).Returns(postedfile.Object);

            postedfile.Setup(f => f.ContentLength).Returns(8192);
            postedfile.Setup(f => f.FileName).Returns("foo.jpg");

            mockSettingRepository.SetupGet(s => s.OriginalImageRoot).Returns("~/Images/Upload/Original");

            var controller = new ImageController(
                imageProcessor ?? mockImageProcessor.Object, 
                imageService ?? mockImageService.Object);

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.SetupGet(x => x.HttpContext).Returns(context.Object);

            controller.ControllerContext = mockControllerContext.Object;

            return controller;
        }
    }
}