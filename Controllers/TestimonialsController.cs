using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChurchOfPantoufle.Data;
using ChurchOfPantoufle.Models;

namespace ChurchOfPantoufle.Controllers;

public class TestimonialsController : Controller
{
    private readonly ChurchContext _context;

    public TestimonialsController(ChurchContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var testimonials = await _context.Testimonials
            .Where(t => t.IsApproved)
            .OrderByDescending(t => t.DateSubmitted)
            .ToListAsync();
        return View(testimonials);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(Testimonial testimonial)
    {
        if (ModelState.IsValid)
        {
            testimonial.DateSubmitted = DateTime.UtcNow;
            testimonial.IsApproved = false;
            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Your testimony has been received by the Warren. The Elder Does will review it in sacred contemplation.";
            return RedirectToAction(nameof(Index));
        }

        var testimonials = await _context.Testimonials
            .Where(t => t.IsApproved)
            .OrderByDescending(t => t.DateSubmitted)
            .ToListAsync();
        return View(nameof(Index), testimonials);
    }
}
