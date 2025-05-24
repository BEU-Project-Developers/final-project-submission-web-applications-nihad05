using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
