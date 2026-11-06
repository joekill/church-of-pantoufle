using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ChurchOfPantoufle.Data;

namespace ChurchOfPantoufle.Controllers;

// The whole area stays disabled (404 at login) unless ADMIN_PASSWORD is set.
[Authorize]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AdminController : Controller
{
    public const string LoginRateLimit = "admin-login";

    private readonly ChurchContext _context;
    private readonly string? _adminPassword;

    public AdminController(ChurchContext context, IConfiguration configuration)
    {
        _context = context;
        _adminPassword = configuration["ADMIN_PASSWORD"];
    }

    private bool IsEnabled => !string.IsNullOrEmpty(_adminPassword);

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (!IsEnabled) return NotFound();
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting(LoginRateLimit)]
    public async Task<IActionResult> Login(string? password)
    {
        if (!IsEnabled) return NotFound();

        if (!PasswordMatches(password ?? string.Empty, _adminPassword!))
        {
            ViewData["LoginError"] = "The Warren does not recognize that password.";
            return View();
        }

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "admin")],
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var testimonials = await _context.Testimonials
            .OrderBy(t => t.IsApproved)
            .ThenByDescending(t => t.DateSubmitted)
            .ToListAsync();
        return View(testimonials);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
        if (testimonial == null) return NotFound();

        testimonial.IsApproved = true;
        await _context.SaveChangesAsync();
        TempData["AdminMessage"] = $"Approved the testimony from {testimonial.AuthorName}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
        if (testimonial == null) return NotFound();

        _context.Testimonials.Remove(testimonial);
        await _context.SaveChangesAsync();
        TempData["AdminMessage"] = $"Deleted the testimony from {testimonial.AuthorName}.";
        return RedirectToAction(nameof(Index));
    }

    // Hashing first gives equal-length inputs, so the comparison reveals nothing about length.
    private static bool PasswordMatches(string supplied, string expected) =>
        CryptographicOperations.FixedTimeEquals(
            SHA256.HashData(Encoding.UTF8.GetBytes(supplied)),
            SHA256.HashData(Encoding.UTF8.GetBytes(expected)));
}
