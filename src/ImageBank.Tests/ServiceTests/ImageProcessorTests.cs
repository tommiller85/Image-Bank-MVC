using System.IO;
using ImageBank.Core;
using ImageBank.Persistence;
using ImageBank.Services.ImageProcessing;
using ImageBank.Services.Virtual;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ServiceTests
{
    [TestFixture]
    public class ImageProcessorTests
    {
        [Test]
        public void ProcessImageChunk_WhenChunkIsNull_ShouldSaveMetadata()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(x => x.Add(It.IsAny<Image>())).Verifiable();
            var imageProcessor = GetImageProcessor(imageRepository: mockImageRepository.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = null, Chunks = null, SystemFilename = "foo.jpg"});

            mockImageRepository.Verify(x => x.Add(It.IsAny<Image>()));
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsZero_ShouldSaveMetadata()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(x => x.Add(It.IsAny<Image>())).Verifiable();
            var imageProcessor = GetImageProcessor(imageRepository: mockImageRepository.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 0, Chunks = 0, SystemFilename = "foo.jpg"});

            mockImageRepository.Verify(x => x.Add(It.IsAny<Image>()));
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsNotZero_ShouldNotSaveMetadata()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository.Setup(x => x.Add(It.IsAny<Image>())).Verifiable();
            var imageProcessor = GetImageProcessor(imageRepository: mockImageRepository.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 1, Chunks = 3, SystemFilename = "foo.jpg"});

            mockImageRepository.Verify(x => x.Add(It.IsAny<Image>()), Times.Never());
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsZero_ShouldCreateImage()
        {
            var mockImageChunkSaver = new Mock<IImageChunkSaver>();
            mockImageChunkSaver.Setup(x => x.SaveImageChunk(It.IsAny<ImageChunk>(), FileMode.Create, It.IsAny<string>()))
                .Verifiable();
            var imageProcessor = GetImageProcessor(imageChunkSaver: mockImageChunkSaver.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 0, SystemFilename = "foo.jpg"});

            mockImageChunkSaver.Verify(
                img => img.SaveImageChunk(It.IsAny<ImageChunk>(), FileMode.Create, It.IsAny<string>()));
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsNotZero_ShouldAppendToImage()
        {
            var mockImageChunkSaver = new Mock<IImageChunkSaver>();
            mockImageChunkSaver.Setup(
                x => x.SaveImageChunk(new ImageChunk(), FileMode.Append, "C:\\Images\\Upload\\Original")).Verifiable();
            var imageProcessor = GetImageProcessor(imageChunkSaver: mockImageChunkSaver.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 1, Chunks = 2, SystemFilename = "foo.jpg"});

            mockImageChunkSaver.Verify(
                img => img.SaveImageChunk(It.IsAny<ImageChunk>(), FileMode.Append, It.IsAny<string>()));
        }

        [Test]
        public void ProcessImageChunk_WhenSavingImage_ShouldSaveToOriginalImageRoot()
        {
            var mockImageChunkSaver = new Mock<IImageChunkSaver>();
            mockImageChunkSaver.Setup(
                x => x.SaveImageChunk(new ImageChunk(), FileMode.Append, "C:\\WebRoot\\Images\\Upload\\Original")).
                Verifiable();
            var imageProcessor = GetImageProcessor(imageChunkSaver: mockImageChunkSaver.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 1, Chunks = 2, SystemFilename = "foo.jpg"});

            mockImageChunkSaver.Verify(
                img =>
                img.SaveImageChunk(It.IsAny<ImageChunk>(), It.IsAny<FileMode>(), "C:\\WebRoot\\Images\\Upload\\Original"));
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsNotEqualToOneLessThanChunks_ShouldNotGenerateMipMap()
        {
            var mockMipMapGenerator = new Mock<IMipMapGenerator>();
            mockMipMapGenerator.Setup(x => x.GenerateMipMap(string.Empty, new MipMap(), string.Empty, string.Empty)).
                Verifiable();
            var imageProcessor = GetImageProcessor(mockMipMapGenerator.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 0, Chunks = 3, SystemFilename = "foo.jpg"});

            mockMipMapGenerator.Verify(
                x => x.GenerateMipMap(It.IsAny<string>(), It.IsAny<MipMap>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never());
        }

        [Test]
        public void ProcessImageChunk_WhenChunkIsEqualToOneLessThanChunks_ShouldGenerateMipMap()
        {
            var mockMipMapGenerator = new Mock<IMipMapGenerator>();
            mockMipMapGenerator.Setup(x => x.GenerateMipMap(string.Empty, new MipMap(), string.Empty, string.Empty)).
                Verifiable();
            var imageProcessor = GetImageProcessor(mockMipMapGenerator.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 1, Chunks = 2, SystemFilename = "foo.jpg"});

            mockMipMapGenerator.Verify(
                x => x.GenerateMipMap(It.IsAny<string>(), It.IsAny<MipMap>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ProcessImageChunk_WhenChunkAndChunksAreEqualToZero_ShouldGenerateMipMap()
        {
            var mockMipMapGenerator = new Mock<IMipMapGenerator>();
            mockMipMapGenerator.Setup(x => x.GenerateMipMap(string.Empty, new MipMap(), string.Empty, string.Empty)).
                Verifiable();
            var imageProcessor = GetImageProcessor(mockMipMapGenerator.Object);

            imageProcessor.ProcessImageChunk(new ImageChunk {Chunk = 0, Chunks = 0, SystemFilename = "foo.jpg"});

            mockMipMapGenerator.Verify(
                x => x.GenerateMipMap(It.IsAny<string>(), It.IsAny<MipMap>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.AtLeastOnce());
        }

        private ImageProcessor GetImageProcessor(
            IMipMapGenerator mipMapGenerator = null,
            IImageRepository imageRepository = null,
            ISettingRepository settingRepository = null,
            IImageChunkSaver imageChunkSaver = null,
            IVirtualPathFinder virtualPathFinder = null)
        {
            var mockMipMapGenerator = new Mock<IMipMapGenerator>();
            var mockImageRepository = new Mock<IImageRepository>();

            var mockSettingRepository = new Mock<ISettingRepository>();
            mockSettingRepository.SetupGet(x => x.OriginalImageRoot).Returns("~/Images/Upload/Original");
            mockSettingRepository.SetupGet(x => x.MediumImageRoot).Returns("~/Images/Upload/640x427");

            var mockImageChunkSaver = new Mock<IImageChunkSaver>();

            var mockVirtualPathFinder = new Mock<IVirtualPathFinder>();
            mockVirtualPathFinder.Setup(x => x.ResolvePath("~/Images/Upload/Original")).Returns(
                "C:\\WebRoot\\Images\\Upload\\Original");
            mockVirtualPathFinder.Setup(x => x.ResolvePath("~/Images/Upload/640x427")).Returns(
                "C:\\WebRoot\\Images\\Upload\\640x427");

            var imageProcessor = new ImageProcessor(mipMapGenerator ?? mockMipMapGenerator.Object,
                                                    imageRepository ?? mockImageRepository.Object,
                                                    settingRepository ?? mockSettingRepository.Object,
                                                    imageChunkSaver ?? mockImageChunkSaver.Object,
                                                    virtualPathFinder ?? mockVirtualPathFinder.Object);

            return imageProcessor;
        }
    }
}