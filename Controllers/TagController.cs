using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers;

public class TagController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}