using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class DomainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
