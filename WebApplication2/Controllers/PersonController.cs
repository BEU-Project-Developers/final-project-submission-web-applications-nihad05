using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Person")]
    public class PersonController : Controller
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [Route("Users")]
        public IActionResult Users()
        {
            var persons = _context.Persons
                .Include(p => p.Company)
                .ToList();
            return View("~/Views/Admin/Users/Index.cshtml", persons);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/Users/AddUser.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {
            bool emailExists = _context.Persons
                .Any(p => p.Email.ToLower() == person.Email.ToLower());

            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already taken.");
            }

            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Person>();
            person.PasswordHash = passwordHasher.HashPassword(person, person.PasswordHash);

            person.CreatedAt = DateTime.Now;
            person.IsActive = true;
            _context.Persons.Add(person);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var person = _context.Persons.Find(id);
            if (person == null || !person.IsActive)
            {
                return NotFound();
            }
            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Person/Edit.cshtml", person);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingPerson = _context.Persons.Find(id);
                if (existingPerson == null || !existingPerson.IsActive)
                {
                    return NotFound();
                }

                existingPerson.FullName = person.FullName;
                existingPerson.Email = person.Email;
                existingPerson.PasswordHash = person.PasswordHash;
                existingPerson.CompanyId = person.CompanyId;
                existingPerson.IsActive = person.IsActive;
                _context.SaveChanges();
                return RedirectToAction("Users");
            }
            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Person/Edit.cshtml", person);
        }

        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var person = _context.Persons.Find(id);
            if (person != null && person.IsActive)
            {
                person.IsActive = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }
    }

}