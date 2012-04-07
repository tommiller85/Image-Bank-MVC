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

        public ImageController(IImageProcessor imageProcessor, IImageRepository imageRepository, ISettingRepository settingRepository)
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
        public ActionResult Upload(int? chunk, string name)
        {
            chunk = chunk ?? 0;
            var file = Request.Files[0];
            var savePath = Server.MapPath(_settingRepository.ImageRoot);
            var systemFilename = _imageProcessor.ProcessImage(chunk == 0 ? FileMode.Create : FileMode.Append, 
                file.InputStream, 
                name, 
                savePath);

            if (chunk == 0)
            {
                var image = new Image
                                {
                                    Filename = name,
                                    SystemFilename = systemFilename,
                                    UploadDir = savePath,
                                    UploadDate = DateTime.UtcNow
                                };
                _imageRepository.Add(image);
            }

            return Content("chunk uploaded", "text/plain");
        }
    }
}