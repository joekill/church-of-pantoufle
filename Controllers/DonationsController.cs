using Microsoft.AspNetCore.Mvc;
using ChurchOfPantoufle.Models;
using ChurchOfPantoufle.Services;

namespace ChurchOfPantoufle.Controllers;

public class DonationsController : Controller
{
    private readonly IDonationService _donationService;

    public DonationsController(IDonationService donationService)
    {
        _donationService = donationService;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Process(DonationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var success = await _donationService.ProcessDonationAsync(model);
            if (success)
            {
                TempData["DonationSuccess"] = "The Radiant One has felt your offering reverberate through the Warren. May your spirit leap ever higher, generous one.";
                return RedirectToAction(nameof(Index));
            }
        }

        return View(nameof(Index), model);
    }
}
