using System.Web.Mvc;
using ImageBank.Services.Account;
using ImageBank.Web.Models;

namespace ImageBank.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountProvider _accountProvider;

        public AccountController(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl = null)
        {
            var authenticated = _accountProvider.Authenticate(model.Username, model.Password);
            if (!authenticated)
            {
                ModelState.AddModelError("", "Bad username or password combination.");
                return View(model);
            }

            _accountProvider.SetAuthCookie(model.Username, false);

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // TODO: Complete Register page.
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }
    }
}