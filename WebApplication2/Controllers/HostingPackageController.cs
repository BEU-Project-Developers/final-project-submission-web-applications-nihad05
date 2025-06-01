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
            var hostingPackages = await _context.HostingPackages
                .Include(h => h.Company)
                .ToListAsync();
            return View("~/Views/Admin/Hosting.cshtml",hostingPackages);
        }

        // GET: HostingPackage/Create
        public IActionResult Create()
        {
            ViewBag.Companies = new SelectList(_context.Companies.ToList(), "Id", "Name");
            return View("~/Views/Admin/HostingPackage/Create.cshtml");
        }

        // POST: HostingPackage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HostingPackage hostingPackage)
        {
                hostingPackage.CreatedAt = DateTime.UtcNow;
                _context.HostingPackages.Add(hostingPackage);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hosting package created successfully!";
                return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostingPackage = await _context.HostingPackages.FindAsync(id);
            if (hostingPackage == null)
            {
                return NotFound();
            }
            ViewBag.Companies = new SelectList(_context.Companies.ToList(), "Id", "Name", hostingPackage.CompanyId);
            return View("~/Views/Admin/HostingPackage/Edit.cshtml", hostingPackage);
        }

        // POST: HostingPackage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HostingPackage hostingPackage)
        {
            if (id != hostingPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Don't update CreatedAt - keep original value
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
                    ////if (!HostingPackageExists(hostingPackage.Id))
                    ////{
                    ////    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            }

            ViewBag.Companies = new SelectList(_context.Companies.ToList(), "Id", "Name", hostingPackage.CompanyId);
            return View("~/Views/Admin/HostingPackage/Edit.cshtml", hostingPackage);
        }

        private bool HostingPackageExists(int id)
        {
            return _context.HostingPackages.Any(e => e.Id == id);
        }
    }
}