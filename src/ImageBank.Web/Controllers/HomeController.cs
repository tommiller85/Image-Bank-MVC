using System.Linq;
using System.Web.Mvc;
using ImageBank.Core;
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

        public ActionResult Index(int page = 1)
        {
            ViewBag.Message = "Welcome to Image Bank.";

            return View(_imageRepository.GetAll().OrderByDescending(x => x.UploadDate).Take(3).ToList());
        }
    }
}