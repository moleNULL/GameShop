using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IIdentityParser<ApplicationUser> _identityParser;

        public AccountController(
            ILogger<AccountController> logger,
            IIdentityParser<ApplicationUser> identityParser)
        {
            _logger = logger;
            _identityParser = identityParser;
        }

        public IActionResult SignIn()
        {
            var user = _identityParser.Parse(User);

            _logger.LogTrace($"User {user.Name} authenticated");

            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }

        public async Task<IActionResult> Signout()
        {
            var user = _identityParser.Parse(User);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            _logger.LogTrace($"User {user.Name} sign out");

            var homeUrl = Url.Action(nameof(CatalogController.Index), "Catalog");
            return new SignOutResult(
                OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = homeUrl });
        }
    }
}
