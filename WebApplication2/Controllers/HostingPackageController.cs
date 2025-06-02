using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HostingPackageController : Controller
    {
        private readonly AppDbContext _context;

        public HostingPackageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: HostingPackage
        public async Task<IActionResult> Index()
        {
            try
            {
                var hostingPackages = await _context.HostingPackages
                    .Include(h => h.Company)
                    .ToListAsync();
                return View("~/Views/Admin/Hosting.cshtml", hostingPackages);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading hosting packages: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: HostingPackage/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.Companies = new SelectList(_context.Companies.ToList(), "Id", "Name");
                return View("~/Views/Admin/HostingPackage/Create.cshtml");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error preparing create form: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: HostingPackage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HostingPackage hostingPackage)
        {
            try
            {
                hostingPackage.CreatedAt = DateTime.UtcNow;
                _context.HostingPackages.Add(hostingPackage);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hosting package created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating hosting package: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var hostingPackage = await _context.HostingPackages.FindAsync(id);
                if (hostingPackage == null)
                    return NotFound();

                ViewBag.Companies = new SelectList(_context.Companies.ToList(), "Id", "Name", hostingPackage.CompanyId);
                return View("~/Views/Admin/HostingPackage/Edit.cshtml", hostingPackage);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading edit form: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: HostingPackage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HostingPackage hostingPackage)
        {
            if (id != hostingPackage.Id)
                return NotFound();

            try
            {
                var existingPackage = await _context.HostingPackages.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (existingPackage != null)
                {
                    hostingPackage.CreatedAt = existingPackage.CreatedAt;
                }

                _context.Update(hostingPackage);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hosting package updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostingPackageExists(hostingPackage.Id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating hosting package: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        private bool HostingPackageExists(int id)
        {
            return _context.HostingPackages.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var hostingPackage = await _context.HostingPackages
                    .Include(h => h.Company)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (hostingPackage == null)
                    return NotFound();

                return View("~/Views/Admin/HostingPackage/Details.cshtml", hostingPackage);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading details: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var package = _context.HostingPackages.Find(id);
                if (package != null)
                {
                    package.Status = 0;
                    _context.SaveChanges();
                    TempData["Success"] = "Hosting package deleted (soft) successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting hosting package: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
