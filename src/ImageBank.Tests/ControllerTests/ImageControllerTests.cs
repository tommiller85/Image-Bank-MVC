using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ImageBank.Core.ImageProcessing;
using ImageBank.Persistence;
using ImageBank.Web.Controllers;
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

            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void UploadPost_ShouldReturn_ContentResult()
        {
            var controller = GetImageController();

            var result = controller.Upload(0, "foo.jpg");

            Assert.IsInstanceOf(typeof(ContentResult), result);
        }

        [Test]
        public void UploadPost_WhenChunkIsZero_ShouldCreateFile()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.ProcessImage(FileMode.Create, It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(0, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.ProcessImage(FileMode.Create, null, "foo.jpg", "C:/Images/Upload"));
        }

        [Test]
        public void UploadPost_WhenChunkIsNotZero_ShouldAppendToFile()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.ProcessImage(FileMode.Create, It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(1, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.ProcessImage(FileMode.Append, null, "foo.jpg", "C:/Images/Upload"));
        }

        [Test]
        public void UploadPost_WhenChunkIsZero_ShouldSaveMetaData()
        {
            
        }

        private ImageController GetImageController(IImageProcessor imageProcessor = null, 
            IImageRepository imageRepository = null)
        {
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            var postedfile = new Mock<HttpPostedFileBase>();
            var postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();

            var mockImageProcessor = new Mock<IImageProcessor>();
            var mockImageRepository = new Mock<IImageRepository>();
            var mockSettingRepository = new Mock<ISettingRepository>();

            var fakeFileKeys = new List<string> {"file"};

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Server.MapPath("~/Images/Upload")).Returns("C:/Images/Upload");

            request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);

            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys[0]).Returns(postedfile.Object);

            postedfile.Setup(f => f.ContentLength).Returns(8192);
            postedfile.Setup(f => f.FileName).Returns("foo.jpg");

            mockSettingRepository.SetupGet(s => s.ImageRoot).Returns("~/Images/Upload");

            var controller = new ImageController(imageProcessor ?? mockImageProcessor.Object,
                                                 imageRepository ?? mockImageRepository.Object,
                                                 mockSettingRepository.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            return controller;
        }
    }
}
