using System.Linq;
using System.Web.Mvc;
using ImageBank.Persistence;

namespace ImageBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageRepository _imageRepository;

        public HomeController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Image Bank.";

            var images = _imageRepository.GetAll().OrderByDescending(x => x.UploadDate);

            return View(images);
        }
    }
}