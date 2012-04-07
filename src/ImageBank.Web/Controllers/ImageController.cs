using System;
using System.IO;
using System.Web.Mvc;
using ImageBank.Core;
using ImageBank.Core.ImageProcessing;
using ImageBank.Persistence;

namespace ImageBank.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IImageRepository _imageRepository;
        private readonly ISettingRepository _settingRepository;

        public ImageController(
            IImageProcessor imageProcessor, 
            IImageRepository imageRepository, 
            ISettingRepository settingRepository)
        {
            _imageProcessor = imageProcessor;
            _imageRepository = imageRepository;
            _settingRepository = settingRepository;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(int? chunk, int? chunks, string name)
        {
            chunk = chunk ?? 0;
            var file = Request.Files[0];
            var savePath = Server.MapPath(_settingRepository.OriginalImageRoot);

            // If this is the first or only chunk then save the image metadata.
            if (chunk == 0)
            {
                SaveImageMetadata(name, name, savePath);
            }

            // Process and save the image in chunks.
            ProcessChunkedImage(chunk, chunks, name, file.InputStream, savePath);

            // todo: Implement this.
            // If we're on the final chunk we can read the file and create our minimaps.
            if (chunk == chunks)
            {
                _imageProcessor.GenerateMipMaps();
            }

            return Content("chunk uploaded", "text/plain");
        }

        private void SaveImageMetadata(string filename, string systemFilename, string uploadDir)
        {
            var image = new Image
            {
                Filename = filename,
                SystemFilename = systemFilename,
                UploadDir = uploadDir,
                UploadDate = DateTime.UtcNow
            };
            _imageRepository.Add(image);
        }

        private void ProcessChunkedImage(int? chunk, int? chunks, string filename, Stream inputStream, string savePath)
        {
            var imageChunk = new ImageChunk
            {
                Chunk = chunk,
                Chunks = chunks,
                Filename = filename,
                InputStream = inputStream
            };
            _imageProcessor.ProcessChunkedImage(imageChunk, chunk == 0 ? FileMode.Create : FileMode.Append, savePath);
        }
    }
}