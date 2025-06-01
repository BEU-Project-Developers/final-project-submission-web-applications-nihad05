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
            var domains = await _context.Domains
                .Include(d => d.Company)
                .OrderBy(d => d.DomainName)
                .ToListAsync();

            return View(domains);
        }
    }
}