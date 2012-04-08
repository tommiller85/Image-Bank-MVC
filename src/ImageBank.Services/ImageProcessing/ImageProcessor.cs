using System;
using System.Drawing.Imaging;
using System.IO;
using ImageBank.Core;
using ImageBank.Persistence;
using ImageBank.Services.Virtual;

namespace ImageBank.Services.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageResizer _imageResizer;
        private readonly IImageRepository _imageRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IImageChunkSaver _imageChunkSaver;
        private readonly IVirtualPathFinder _virtualPathFinder;

        public ImageProcessor(
            IImageResizer imageResizer,
            IImageRepository imageRepository,
            ISettingRepository settingRepository,
            IImageChunkSaver imageChunkSaver,
            IVirtualPathFinder virtualPathFinder)
        {
            _imageResizer = imageResizer;
            _imageRepository = imageRepository;
            _settingRepository = settingRepository;
            _imageChunkSaver = imageChunkSaver;
            _virtualPathFinder = virtualPathFinder;
        }

        public void ProcessImageChunk(ImageChunk imageChunk)
        {
            if (imageChunk.Chunk == null)
                imageChunk.Chunk = 0;
            if (imageChunk.Chunks == null)
                imageChunk.Chunks = 0;

            SaveImageMetadata(imageChunk);
            SaveImageChunk(imageChunk);
            GenerateMipMaps(imageChunk);
        }

        private void SaveImageMetadata(ImageChunk imageChunk)
        {
            if (imageChunk.Chunk == 0)
            {
                var image = new Image
                                {
                                    Filename = imageChunk.SystemFilename,
                                    SystemFilename = imageChunk.SystemFilename,
                                    UploadDate = DateTime.UtcNow
                                };
                _imageRepository.Add(image);
            }
        }

        private void SaveImageChunk(ImageChunk imageChunk)
        {
            _imageChunkSaver.SaveImageChunk(imageChunk, imageChunk.Chunk == 0 ? FileMode.Create : FileMode.Append,
                                            _virtualPathFinder.ResolvePath(_settingRepository.OriginalImageRoot));
        }

        //TODO: This needs refactoring and test coverage.
        private void GenerateMipMaps(ImageChunk imageChunk)
        {
            if (imageChunk.Chunks == 0 || imageChunk.Chunk == (imageChunk.Chunks - 1))
            {
                ImageCodecInfo info = ImageResizer.ProcessCodecs("image/jpeg"); // todo: fix this

                _imageResizer.GenerateMipMap(
                    Path.Combine(_virtualPathFinder.ResolvePath(_settingRepository.OriginalImageRoot),
                                 imageChunk.SystemFilename),
                    new MipMap {Codec = info, Width = 640, Height = 427},
                    _virtualPathFinder.ResolvePath(_settingRepository.MediumImageRoot),
                    imageChunk.SystemFilename);
            }
        }
    }
}