using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Domain")]
    public class DomainsController : Controller
    {
        private readonly AppDbContext _context;

        public DomainsController(AppDbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // API endpoint for fetching domains
        [Route("api/recent")]
        [HttpGet]
        public async Task<IActionResult> GetRecentDomains()
        {
            try
            {
                var domains = await _context.Domains
                    .Include(d => d.Company)
                    .OrderByDescending(d => d.PurchaseDate)
                    .Select(d => new
                    {
                        id = d.Id,
                        domainName = d.DomainName,
                        price = d.Price,
                        purchaseDate = d.PurchaseDate,
                        expiryDate = d.ExpiryDate,
                        isActive = d.IsActive,
                        company = d.Company != null ? new { name = d.Company.Name } : null
                    })
                    .ToListAsync();

                return Json(domains);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load domains" });
            }
        }
    }
}