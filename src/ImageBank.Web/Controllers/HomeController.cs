using System.Web.Mvc;
using ImageBank.Services.Image;

namespace ImageBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;

        public HomeController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public ActionResult Index()
        {
            var homepageImages = _imageService.GetHomepageImages();
            
            return View(homepageImages);
        }
    }
}