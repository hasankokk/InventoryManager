using Microsoft.AspNetCore.Mvc;
using InventoryManager.Data;
using InventoryManager.Models.Entities;
using InventoryManager.Models.DTOs;
using InventoryManager.Models.DTOs.Container;
using Mapster;

namespace InventoryManager.Controllers;

[ApiController]
public class ContainerController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("/Container")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddContainer(ContainerCreateDto dto)
    {
        var newContainer = new Container
        {
            Name = dto.Name,
            LocationId = dto.LocationId,
        };
        context.Containers.Add(newContainer);
        context.SaveChanges();
        return Created();
    }

    [HttpGet]
    [Route("/Container")]
    [ProducesResponseType<ContainerListWithLocation[]>(StatusCodes.Status200OK)]    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetContainer(int id)
    {
        var container = context.Containers.Find(id);
        if (container == null)
        {
            return NotFound("Container not found");
        }
        var listContainer = container.Adapt<ContainerListWithLocation>();
        return Ok(listContainer);
    }
}