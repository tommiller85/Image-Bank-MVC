using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ImageBank.Core;
using ImageBank.Services.Image;
using ImageBank.Services.ImageProcessing;
using ImageBank.Web.Models;

namespace ImageBank.Web.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IImageService _imageService;

        public ImageController(
            IImageProcessor imageProcessor,
            IImageService imageService)
        {
            _imageProcessor = imageProcessor;
            _imageService = imageService;
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

            _imageProcessor.ProcessImageChunk(imageChunk, User.Identity.Name);

            return Content("chunk uploaded", "text/plain");
        }

        [HttpGet]
        public ActionResult EditImages(int pageIndex = 0, int pageSize = 5)
        {
            var pageOfImages = _imageService.GetImagesByUser(User.Identity.Name, pageIndex, pageSize);
            var model = pageOfImages.Select(x =>
                        new EditImageModel
                            {
                                Id = x.Id,
                                Filename = x.Filename,
                                SystemFilename = x.SystemFilename,
                                Description = x.Description,
                                IsPublic = x.IsPublic,
                                ShowOnHomepage = x.ShowOnHomePage
                            });

            ViewData["PageIndex"] = pageIndex;
            ViewData["HasPrevious"] = pageOfImages.HasPreviousPage;
            ViewData["HasNext"] = pageOfImages.HasNextPage;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditImages(IEnumerable<EditImageModel> images, int pageIndex = 0, int pageSize = 5)
        {
            if(!ModelState.IsValid)
            {
                return EditImages(pageIndex, pageSize);
            }

            List<Image> imagesToUpdate = new List<Image>();
            foreach(var image in images)
            {
                var imageToUpdate = _imageService.GetImageById(image.Id);

                imageToUpdate.Filename = image.Filename;
                imageToUpdate.Description = image.Description;
                imageToUpdate.IsPublic = image.IsPublic;
                imageToUpdate.ShowOnHomePage = image.ShowOnHomepage;
                imageToUpdate.Deleted = image.Delete;

                imagesToUpdate.Add(imageToUpdate);
            }
            _imageService.EditImages(imagesToUpdate);

            return RedirectToAction("EditImages", "Image", new { pageIndex });
        }
    }
}