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
        private readonly IMipMapGenerator _mipMapGenerator;
        private readonly IImageRepository _imageRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IImageChunkSaver _imageChunkSaver;
        private readonly IVirtualPathResolver _virtualPathResolver;

        public ImageProcessor(
            IMipMapGenerator mipMapGenerator,
            IImageRepository imageRepository,
            ISettingRepository settingRepository,
            IImageChunkSaver imageChunkSaver,
            IVirtualPathResolver virtualPathResolver)
        {
            _mipMapGenerator = mipMapGenerator;
            _imageRepository = imageRepository;
            _settingRepository = settingRepository;
            _imageChunkSaver = imageChunkSaver;
            _virtualPathResolver = virtualPathResolver;
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
                                            _virtualPathResolver.ResolvePath(_settingRepository.OriginalImageRoot));
        }

        //TODO: This needs refactoring and test coverage.
        private void GenerateMipMaps(ImageChunk imageChunk)
        {
            if (imageChunk.Chunks == 0 || imageChunk.Chunk == (imageChunk.Chunks - 1))
            {
                ImageCodecInfo info = MipMapGenerator.ProcessCodecs("image/jpeg"); // todo: fix this

                _mipMapGenerator.GenerateMipMap(
                    Path.Combine(_virtualPathResolver.ResolvePath(_settingRepository.OriginalImageRoot),
                                 imageChunk.SystemFilename),
                    new MipMap {Codec = info, Width = 640, Height = 427},
                    _virtualPathResolver.ResolvePath(_settingRepository.MediumImageRoot),
                    imageChunk.SystemFilename);
            }
        }
    }
}