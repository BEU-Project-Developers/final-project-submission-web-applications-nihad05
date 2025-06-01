using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization; // Add this using

namespace WebApplication2.Controllers
{
    [AllowAnonymous] // Add this attribute
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // If already authenticated, redirect to home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Persons
                    .Include(p => p.Company)
                    .FirstOrDefault(p => p.Email == model.Email && p.IsActive);

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<Person>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FullName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("UserId", user.Id.ToString()),
                            new Claim("CompanyName", user.Company?.Name ?? "No Company"),
                            new Claim("CompanyId", user.CompanyId.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = false,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login"); // Changed from Home to Login
        }
    }
}