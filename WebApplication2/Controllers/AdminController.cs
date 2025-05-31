using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
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

        [Route("Users/Add")]
        public IActionResult UsersAdd()
        {
            return View("~/Views/Admin/Users/AddUser.cshtml");
        }

        [Route("Company")]
        public IActionResult Company()
        {
            var companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/Company/Index.cshtml", companies);
        }

        [Route("Company/Add")]
        public IActionResult CompanyAdd()
        {
            return View("~/Views/Admin/Company/AddCompany.cshtml");
        }

        [HttpPost]
        [Route("Company/Add")]
        [ValidateAntiForgeryToken]
        public IActionResult CompanyAdd(Company company)
        {
            if (ModelState.IsValid)
            {
                company.CreatedAt = DateTime.Now;
                _context.Companies.Add(company);
                _context.SaveChanges();
                return RedirectToAction("Company");
            }
            return View("~/Views/Admin/Company/AddCompany.cshtml", company);
        }

        [HttpPost]
        [Route("Company/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Find(id);
            if (company != null && company.DeletedAt == null)
            {
                company.DeletedAt = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("Company");
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

        // Company Edit

        [HttpGet]
        [Route("Company/Edit/{id}")]
        public IActionResult EditCompany(int id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);
            if (company == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/Company/EditCompany.cshtml", company);
        }

        [HttpPost]
        [Route("Company/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingCompany = _context.Companies.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);
                if (existingCompany == null)
                {
                    return NotFound();
                }

                existingCompany.Name = company.Name;
                existingCompany.Email = company.Email;
                existingCompany.Phone = company.Phone;
                existingCompany.Address = company.Address;
                existingCompany.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return RedirectToAction("Company");
            }

            return View("~/Views/Admin/Company/EditCompany.cshtml", company);
        }
    }
}
