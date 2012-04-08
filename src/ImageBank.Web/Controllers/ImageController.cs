using System;
using System.Web.Mvc;
using ImageBank.Services.ImageProcessing;

namespace ImageBank.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageProcessor _imageProcessor;

        public ImageController(
            IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(int? chunk, int? chunks, string name)
        {
            var file = Request.Files[0];
            if(file == null)
            {
                throw new InvalidOperationException("no file process");
            }

            var imageChunk = new ImageChunk
            {
                Chunk = chunk,
                Chunks = chunks,
                SystemFilename = name,
                InputStream = file.InputStream
            };

            _imageProcessor.ProcessImageChunk(imageChunk);

            return Content("chunk uploaded", "text/plain");
        }
    }
}