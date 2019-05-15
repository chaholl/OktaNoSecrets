using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace OktaNoSecrets.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}