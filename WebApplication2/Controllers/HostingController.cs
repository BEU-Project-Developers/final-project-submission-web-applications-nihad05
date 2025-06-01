using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Hosting")]
    public class HostingController : Controller
    {
        private readonly AppDbContext _context;

        public HostingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Packages")]
        public async Task<IActionResult> Packages()
        {
            return Json("here");
            var hostingPackages = await _context.HostingPackages
                .Include(h => h.Company)
                .OrderBy(h => h.Price)
                .ToListAsync();

            return View("~/Views/Admin/Hosting.cshtml", hostingPackages);
        }

        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var hostingPackage = await _context.HostingPackages
                .Include(h => h.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hostingPackage == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/HostingDetails.cshtml", hostingPackage);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/AdminHosting/Create.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StorageGB,BandwidthGB,Price,Status,Description,CompanyId")] HostingPackage hostingPackage)
        {
            if (ModelState.IsValid)
            {
                hostingPackage.CreatedAt = DateTime.UtcNow;
                _context.Add(hostingPackage);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hosting package created successfully!";
                return RedirectToAction("Packages");
            }

            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/AdminHosting/Create.cshtml", hostingPackage);
        }

        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var hostingPackage = await _context.HostingPackages.FindAsync(id);
            if (hostingPackage == null)
            {
                return NotFound();
            }

            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/AdminHosting/Edit.cshtml", hostingPackage);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StorageGB,BandwidthGB,Price,Status,Description,CompanyId,CreatedAt")] HostingPackage hostingPackage)
        {
            if (id != hostingPackage.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPackage = await _context.HostingPackages.FindAsync(id);
                    if (existingPackage == null)
                    {
                        return NotFound();
                    }

                    existingPackage.Name = hostingPackage.Name;
                    existingPackage.StorageGB = hostingPackage.StorageGB;
                    existingPackage.BandwidthGB = hostingPackage.BandwidthGB;
                    existingPackage.Price = hostingPackage.Price;
                    existingPackage.Status = hostingPackage.Status;
                    existingPackage.Description = hostingPackage.Description;
                    existingPackage.CompanyId = hostingPackage.CompanyId;

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Hosting package updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HostingPackageExists(hostingPackage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Packages");
            }

            ViewBag.Companies = _context.Companies.Where(c => c.DeletedAt == null).ToList();
            return View("~/Views/Admin/AdminHosting/Edit.cshtml", hostingPackage);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hostingPackage = await _context.HostingPackages
                .Include(h => h.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hostingPackage == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/AdminHosting/Delete.cshtml", hostingPackage);
        }

        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hostingPackage = await _context.HostingPackages.FindAsync(id);
            if (hostingPackage != null)
            {
                // Soft delete by setting Status to false instead of hard delete
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hosting package deleted successfully!";
            }

            return RedirectToAction("Packages");
        }

        private bool HostingPackageExists(int id)
        {
            return _context.HostingPackages.Any(e => e.Id == id);
        }
    }
}