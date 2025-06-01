using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Admin/Domains")]
    public class AdminDomainController : Controller
    {
        private readonly AppDbContext _context;

        public AdminDomainController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Domains
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var domains = await _context.Domains
                .Include(d => d.Company)
                .OrderBy(d => d.DomainName)
                .ToListAsync();

            return View("~/Views/Admin/Domains/Index.cshtml", domains);
        }

        // GET: Admin/Domains/Details/5
        [Route("Details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domains
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (domain == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Domains/Details.cshtml", domain);
        }

        // GET: Admin/Domains/Create
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Companies = await _context.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View("~/Views/Admin/Domains/Create.cshtml");
        }

        // POST: Admin/Domains/Create
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DomainName,Price,PurchaseDate,ExpiryDate,IsActive,CompanyId")] Domain domain)
        {
                // Check if domain name already exists
                var existingDomain = await _context.Domains
                    .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.DomainName.ToLower());

                if (existingDomain != null)
                {
                    ModelState.AddModelError("DomainName", "This domain name already exists in the system.");
                }
                else
                {
                    // Validate expiry date is after purchase date
                    if (domain.ExpiryDate <= domain.PurchaseDate)
                    {
                        ModelState.AddModelError("ExpiryDate", "Expiry date must be after the purchase date.");
                    }
                    else
                    {
                        _context.Add(domain);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = $"Domain '{domain.DomainName}' has been created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }

            // Reload companies for dropdown if validation fails
            ViewBag.Companies = await _context.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View("~/Views/Admin/Domains/Create.cshtml", domain);
        }

        [Route("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var domain = await _context.Domains.FindAsync(id);
            //if (domain == null)
            //{
            //    return NotFound();
            //}

            //// Add defensive programming and async call
            //var companies = await _context.Companies.ToListAsync();
            //if (companies == null)
            //{
            //    companies = new List<Company>();
            //}

            //ViewBag.Companies = new SelectList(companies, "Id", "Name", domain.CompanyId);
            return View("~/Views/Admin/Domains/Edit.cshtml");
        }

        // POST: Admin/Domains/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DomainName,Price,PurchaseDate,ExpiryDate,IsActive,CompanyId")] Domain domain)
        {
            if (id != domain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if domain name already exists (excluding current domain)
                    var existingDomain = await _context.Domains
                        .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.DomainName.ToLower() && d.Id != domain.Id);

                    if (existingDomain != null)
                    {
                        ModelState.AddModelError("DomainName", "This domain name already exists in the system.");
                    }
                    else
                    {
                        // Validate expiry date is after purchase date
                        if (domain.ExpiryDate <= domain.PurchaseDate)
                        {
                            ModelState.AddModelError("ExpiryDate", "Expiry date must be after the purchase date.");
                        }
                        else
                        {
                            _context.Update(domain);
                            await _context.SaveChangesAsync();
                            TempData["SuccessMessage"] = $"Domain '{domain.DomainName}' has been updated successfully.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomainExists(domain.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Reload companies for dropdown if validation fails
            ViewBag.Companies = await _context.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View("~/Views/Admin/Domains/Edit.cshtml", domain);
        }

        // GET: Admin/Domains/Delete/5
        [Route("Delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domain = await _context.Domains
                .Include(d => d.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (domain == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Domains/Delete.cshtml", domain);
        }

        // POST: Admin/Domains/Delete/5
        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var domain = await _context.Domains.FindAsync(id);
            if (domain != null)
            {
                _context.Domains.Remove(domain);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Domain '{domain.DomainName}' has been deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Domains/ExpiringSoon
        [Route("ExpiringSoon")]
        public async Task<IActionResult> ExpiringSoon(int days = 30)
        {
            var cutoffDate = DateTime.Now.AddDays(days);
            var expiringDomains = await _context.Domains
                .Include(d => d.Company)
                .Where(d => d.IsActive && d.ExpiryDate <= cutoffDate && d.ExpiryDate >= DateTime.Now)
                .OrderBy(d => d.ExpiryDate)
                .ToListAsync();

            ViewBag.Days = days;
            return View("~/Views/Admin/Domains/ExpiringSoon.cshtml", expiringDomains);
        }

        // GET: Admin/Domains/Expired
        [Route("Expired")]
        public async Task<IActionResult> Expired()
        {
            var expiredDomains = await _context.Domains
                .Include(d => d.Company)
                .Where(d => d.ExpiryDate < DateTime.Now)
                .OrderBy(d => d.ExpiryDate)
                .ToListAsync();

            return View("~/Views/Admin/Domains/Expired.cshtml", expiredDomains);
        }

        // GET: Admin/Domains/ByCompany/5
        [Route("ByCompany/{companyId}")]
        public async Task<IActionResult> ByCompany(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            var domains = await _context.Domains
                .Include(d => d.Company)
                .Where(d => d.CompanyId == companyId)
                .OrderBy(d => d.DomainName)
                .ToListAsync();

            ViewBag.CompanyName = company.Name;
            return View("~/Views/Admin/Domains/ByCompany.cshtml", domains);
        }

        // POST: Admin/Domains/ToggleStatus/5
        [HttpPost]
        [Route("ToggleStatus/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var domain = await _context.Domains.FindAsync(id);
            if (domain == null)
            {
                return NotFound();
            }

            domain.IsActive = !domain.IsActive;
            _context.Update(domain);
            await _context.SaveChangesAsync();

            var status = domain.IsActive ? "activated" : "deactivated";
            TempData["SuccessMessage"] = $"Domain '{domain.DomainName}' has been {status} successfully.";

            return RedirectToAction(nameof(Index));
        }

        // API endpoint for domain statistics
        [HttpGet]
        [Route("GetStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = new
            {
                TotalDomains = await _context.Domains.CountAsync(),
                ActiveDomains = await _context.Domains.CountAsync(d => d.IsActive),
                ExpiredDomains = await _context.Domains.CountAsync(d => d.ExpiryDate < DateTime.Now),
                ExpiringSoon = await _context.Domains.CountAsync(d => d.IsActive && d.ExpiryDate <= DateTime.Now.AddDays(30) && d.ExpiryDate >= DateTime.Now),
                TotalValue = await _context.Domains.SumAsync(d => d.Price)
            };

            return Json(stats);
        }

        private bool DomainExists(int id)
        {
            return _context.Domains.Any(e => e.Id == id);
        }
    }
}