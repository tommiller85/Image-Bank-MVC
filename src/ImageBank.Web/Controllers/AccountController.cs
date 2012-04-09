using System.Web.Mvc;
using ImageBank.Web.Models;

namespace ImageBank.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel());
        }

        // TODO: Complete Register page.
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }
    }
}