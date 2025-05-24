using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        [Route("Dashboard")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Dashboard/Index.cshtml");
        }

        [Route("Users")]
        public IActionResult Users()
        {
            return View("~/Views/Admin/Users/Index.cshtml");
        }

        [Route("Insanlar")]
        public IActionResult Insanlar()
        {
            return View("~/Views/Admin/Insanlar/Index.cshtml");
        }

        [Route("Users/Add")]
        public IActionResult UsersAdd()
        {
            return View("~/Views/Admin/Users/AddUser.cshtml");
        }

        [Route("Company")]
        public IActionResult Company()
        {
            return View("~/Views/Admin/Company/Index.cshtml");
        }

        [Route("Company/Add")]
        public IActionResult CompanyAdd()
        {
            return View("~/Views/Admin/Company/AddCompany.cshtml");
        }

        [Route("UsersNew")]
        public IActionResult UsersNew()
        {
            return View("~/Views/Admin/UsersNew/Index.cshtml");
        }

        [Route("Domains")]
        public IActionResult Domains()
        {
            return View("~/Views/Admin/Domains/Index.cshtml");
        }
    }
}
