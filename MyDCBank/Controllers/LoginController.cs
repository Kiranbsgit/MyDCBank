using Microsoft.AspNetCore.Mvc;

namespace MyDCBank.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
