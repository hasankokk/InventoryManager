using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers;

public class LocationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}