using Microsoft.AspNetCore.Mvc;
using InventoryManager.Data;
using InventoryManager.Models.Entities;
using InventoryManager.Models.DTOs;
using InventoryManager.Models.DTOs.Container;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers;

[ApiController]
[Route("/container")]
[Authorize(Policy = "Administrator")]
public class ContainerController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Add(ContainerCreateDto dto)
    {
        if (!context.Locations.Any(x => x.Id == dto.LocationId))
            return StatusCode(StatusCodes.Status409Conflict);
        var newContainer = new Container
        {
            Name = dto.Name,
            LocationId = dto.LocationId,
        };
        context.Containers.Add(newContainer);
        context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = newContainer.Id }, newContainer);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType<ContainerListWithLocation[]>(StatusCodes.Status200OK)]    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(int id)
    {
        var container = context.Containers
            .Where(x => x.Id == id)
            .Include(x=>x.Location)
            .FirstOrDefault();
        if (container is null)
        {
            return NotFound("Container not found");
        }
        var listContainer = container.Adapt<ContainerListWithLocation>();
        return Ok(listContainer);
    }

    [HttpPut]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, ContainerUpdateDto dto)
    {
        var container = context.Containers.Find(id);
        if  (container is null)
            return NotFound("Container not found");
        container.Name = dto.Name;
        container.LocationId = dto.LocationId ?? container.LocationId;
        container.Updated = DateTime.Now;
        context.SaveChanges();
        return NoContent();
    }

    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var container = context.Containers.Find(id);
        if (container is null)
            return NotFound("Container not found");
        context.Containers.Remove(container);
        context.SaveChanges();
        return NoContent();
    }
}