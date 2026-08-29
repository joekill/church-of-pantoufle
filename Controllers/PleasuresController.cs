using Microsoft.AspNetCore.Mvc;

namespace ChurchOfPantoufle.Controllers;

public class PleasuresController : Controller
{
    public IActionResult Index() => View();
}
