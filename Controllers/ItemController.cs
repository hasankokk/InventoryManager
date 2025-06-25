using InventoryManager.Data;
using InventoryManager.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Controllers;

[ApiController]
public class ItemController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("/item")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddItem(Item item)
    {
        var newItem = item.Adapt<Item>();
        context.Items.Add(newItem);
        context.SaveChanges();
        return CreatedAtAction(nameof(Index), new { id = newItem.Id }, newItem);
    }
}