using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class HostingController : Controller
    {
        private readonly AppDbContext _context;

        public HostingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userCompanyId = GetCurrentUserCompanyId();
            if (userCompanyId == null)
            {
                TempData["Error"] = "You are not associated with any company.";
                return RedirectToAction("Index", "Home");
            }

            var hostingPackages = await _context.HostingPackages
                .Where(hp => hp.Status == 1)
                .Include(hp => hp.Company)
                .OrderByDescending(hp => hp.CreatedAt)
                .Take(3)
                .ToListAsync();

            return View(hostingPackages);
        }

        private int? GetCurrentUserCompanyId()
        {
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (!string.IsNullOrEmpty(companyIdClaim) && int.TryParse(companyIdClaim, out int companyId))
            {
                return companyId;
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                var user = _context.Persons
                    .FirstOrDefault(u => u.Id == userId);
                return user?.CompanyId;
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> BuyPackage(int packageId)
        {
            var userCompanyId = GetCurrentUserCompanyId();
            var package = await _context.HostingPackages
                .FirstOrDefaultAsync(hp => hp.Id == packageId && hp.CompanyId == userCompanyId);

            if (package == null)
            {
                return NotFound();
            }

            TempData["Success"] = $"Successfully purchased {package.Name} package!";
            return RedirectToAction("Index");
        }
    }
}