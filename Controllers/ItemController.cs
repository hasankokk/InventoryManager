using InventoryManager.Data;
using InventoryManager.Models.DTOs.Item;
using InventoryManager.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers;

[ApiController]
[Route("/item")]
public class ItemController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Add(ItemCreateDto dto)
    { 
        if (!context.Containers.Any(c => c.Id == dto.ContainerId))
            return BadRequest("Geçersiz Container.");
        
        if (dto.Tags.Any())
        {
            var existingTagCount = context.Tags.Count(t => dto.Tags.Contains(t.Id));
            if (existingTagCount != dto.Tags.Count)
                return BadRequest("Tag eklenemedi.");
        }
        
        var item = dto.Adapt<Item>();
        item.Tags = context.Tags.Where(x => dto.Tags.Contains(x.Id)).ToList();
        context.Items.Add(item);
        context.SaveChanges();
        return CreatedAtRoute("DefaultApi", new { controller = "Item", id = item.Id }, item);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType<ItemListDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(int id)
    {
        var item = context.Items.Where(x => x.Id == id).Include(t => t.Tags).Include(c => c.Container).FirstOrDefault();
        if (item == null)
            return NotFound();
        var result = item.Adapt<ItemListDto>();
        result.TagName = item.Tags.Select(x => x.Name).ToList();
        return Ok(result);
    }

    [HttpPut]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, ItemUpdateDto dto)
    {
        var item =  context.Items.Where(x => x.Id == id).Include(t => t.Tags).Include(c => c.Container).FirstOrDefault();
        if (item == null)
            return NotFound();
        dto.Adapt(item);
        
        if (dto.Tags != null && dto.Tags.Any())
        {
            var tags = context.Tags
                .Where(t => dto.Tags.Contains(t.Id))
                .ToList();

            if (dto.IsAdded)
            {
                foreach (var tag in tags)
                {
                    if (!item.Tags.Any(t => t.Id == tag.Id))
                        item.Tags.Add(tag);
                }
            }
            else
            {
                foreach (var tag in tags)
                {
                    var existingTag = item.Tags.FirstOrDefault(t => t.Id == tag.Id);
                    if (existingTag != null)
                        item.Tags.Remove(existingTag);
                }
            }
        }
        item.Updated = DateTime.Now;
        context.SaveChanges();
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var item = context.Items.Find(id);
        if (item == null)
            return NotFound();
        context.Items.Remove(item);
        context.SaveChanges();
        return NoContent();
    }
}