using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public async Task<IActionResult> ShowAsync()
        {
            await Task.Delay(0);
            return View();
        }
    }
}
