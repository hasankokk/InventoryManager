using InventoryManager.Data;
using InventoryManager.Models.DTOs.Location;
using InventoryManager.Models.Entities;
using Scalar;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers;
[ApiController]
[Route("/location")]
public class LocationController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Add(LocationCreateDto dto)
    {
        var newLocation = new Location
        {
            Name = dto.Name,
        };
        context.Locations.Add(newLocation);
        context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = newLocation.Id }, newLocation);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType<LocationListWithContainer[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(int id)
    {
        var location = context.Locations
            .Where(x => x.Id == id)
            .Include(c => c.Containers).FirstOrDefault();
        if (location is null)
        {
            return NotFound("Location not found");
        }
        var containers = location.Containers.Select(x=>x.Name).ToArray();
        var listLocation = new LocationListWithContainer
        {
            Name = location.Name,
            ContainerName = containers,
        };
        return Ok(listLocation);
    }

    [HttpPut]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, LocationUpdateDto dto)
    {
        var location = context.Locations.Find(id);
        if (location == null)
            return NotFound("Location not found");
        location.Name = dto.Name;
        location.Updated = DateTime.Now;
        context.SaveChanges();
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var location = context.Locations.Find(id);
        if (location == null)
            return NotFound("Location not found");
        context.Locations.Remove(location);
        context.SaveChanges();
        return NoContent();
    }
}