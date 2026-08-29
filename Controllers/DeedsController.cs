using Microsoft.AspNetCore.Mvc;

namespace ChurchOfPantoufle.Controllers;

public class DeedsController : Controller
{
    public IActionResult Index() => View();
}
