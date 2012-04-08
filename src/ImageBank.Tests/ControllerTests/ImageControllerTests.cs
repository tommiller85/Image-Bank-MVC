using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ImageBank.Core;
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

            var result = controller.Upload(0, 0, "foo.jpg");

            Assert.IsInstanceOf(typeof(ContentResult), result);
        }

        [Test]
        public void UploadPost_WhenChunkIsZero_ShouldCreateFile()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.ProcessChunkedImage(It.IsAny<ImageChunk>(), FileMode.Create, It.IsAny<string>())).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(0, 0, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.ProcessChunkedImage(It.IsAny<ImageChunk>(), FileMode.Create, "C:\\Images\\Upload\\Original"));
        }

        [Test]
        public void UploadPost_WhenChunkIsNotZero_ShouldAppendToFile()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.ProcessChunkedImage(It.IsAny<ImageChunk>(), FileMode.Create, It.IsAny<string>())).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(1, 5, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.ProcessChunkedImage(It.IsAny<ImageChunk>(), FileMode.Append, "C:\\Images\\Upload\\Original"));
        }

        [Test]
        public void UploadPost_WhenChunkIsZero_ShouldSaveMetaData()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(img => img.Add(It.IsAny<Image>())).Verifiable();
            var controller = GetImageController(imageRepository: mockImageRepository.Object);

            var result = controller.Upload(0, 0, "foo.jpg");

            mockImageRepository.Verify(img => img.Add(It.IsAny<Image>()));
        }

        [Test]
        public void UploadPost_WhenChunkIsNotZero_ShouldNotSaveMetaData()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(img => img.Add(It.IsAny<Image>())).Verifiable();
            var controller = GetImageController(imageRepository: mockImageRepository.Object);

            var result = controller.Upload(1, 5, "foo.jpg");

            mockImageRepository.Verify(img => img.Add(It.IsAny<Image>()), Times.Never());
        }

        [Test]
        public void UploadPost_WhenChunkIsNotEqualToChunks_ShouldNotGenerateMipMaps()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.GenerateMipMaps(It.IsAny<string>())).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(1, 5, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.GenerateMipMaps(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void UploadPost_WhenChunkIsEqualToChunks_ShouldGenerateMipMaps()
        {
            var mockImageProcessor = new Mock<IImageProcessor>();
            mockImageProcessor.Setup(img => img.GenerateMipMaps("C:\\Images\\Upload\\Original\\foo.jpg")).Verifiable();
            var controller = GetImageController(mockImageProcessor.Object);

            var result = controller.Upload(5, 5, "foo.jpg") as ContentResult;

            mockImageProcessor.Verify(img => img.GenerateMipMaps("C:\\Images\\Upload\\Original\\foo.jpg"));
        }

        private ImageController GetImageController(
            IImageProcessor imageProcessor = null, 
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
            context.Setup(ctx => ctx.Server.MapPath("~/Images/Upload/Original")).Returns("C:\\Images\\Upload\\Original");

            request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);

            postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());
            postedfilesKeyCollection.Setup(keys => keys[0]).Returns(postedfile.Object);

            postedfile.Setup(f => f.ContentLength).Returns(8192);
            postedfile.Setup(f => f.FileName).Returns("foo.jpg");

            mockSettingRepository.SetupGet(s => s.OriginalImageRoot).Returns("~/Images/Upload/Original");

            var controller = new ImageController(imageProcessor ?? mockImageProcessor.Object,
                                                 imageRepository ?? mockImageRepository.Object,
                                                 mockSettingRepository.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            return controller;
        }
    }
}
