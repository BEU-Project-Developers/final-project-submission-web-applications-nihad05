using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
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


        // GET: HostingPackages
        public async Task<IActionResult> Index2()
        {
            var hostingPackages = await _context.HostingPackages
              .Include(h => h.Company)
                .OrderBy(h => h.Price)
                .ToListAsync();

            return View("~/Views/Admin/Hosting/Index.cshtml", hostingPackages);
        }

        // GET: HostingPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostingPackage = await _context.HostingPackages
                .Include(h => h.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hostingPackage == null)
            {
                return NotFound();
            }

            return View(hostingPackage);
        }

        // GET: HostingPackages/Create
        public IActionResult Create()
        {
            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: HostingPackages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StorageGB,BandwidthGB,Price,Status,Description,Company")] HostingPackage hostingPackage)
        {
            if (ModelState.IsValid)
            {
                hostingPackage.CreatedAt = DateTime.UtcNow;
                _context.Add(hostingPackage);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hosting package created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name", hostingPackage.Company);
            return View(hostingPackage);
        }

        // GET: HostingPackages/Edit/5
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

            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name", hostingPackage.Company);
            return View(hostingPackage);
        }

        // POST: HostingPackages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StorageGB,BandwidthGB,Price,Status,Description,Company,CreatedAt")] HostingPackage hostingPackage)
        {
            if (id != hostingPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hostingPackage);
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
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Companies = new SelectList(_context.Companies, "Id", "Name", hostingPackage.Company);
            return View(hostingPackage);
        }

        // GET: HostingPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostingPackage = await _context.HostingPackages
                .Include(h => h.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hostingPackage == null)
            {
                return NotFound();
            }

            return View(hostingPackage);
        }

        // POST: HostingPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hostingPackage = await _context.HostingPackages.FindAsync(id);
            if (hostingPackage != null)
            {
                _context.HostingPackages.Remove(hostingPackage);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hosting package deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HostingPackageExists(int id)
        {
            return _context.HostingPackages.Any(e => e.Id == id);
        }
    }
}
