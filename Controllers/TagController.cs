using System.Diagnostics;
using InventoryManager.Data;
using InventoryManager.Models.DTOs.Container;
using InventoryManager.Models.DTOs.Item;
using InventoryManager.Models.DTOs.Tag;
using InventoryManager.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers;

[ApiController]
[Route("/tag")]
[Authorize(Policy = "Administrator")]

public class TagController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Add(TagCreateDto dto)
    {
        var newTag = new Tag
        {
            Name = dto.Name,
        };
        context.Tags.Add(newTag);
        context.SaveChanges();
        return Created("", newTag);
    }

    [HttpGet]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType<TagListWithItem>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(int id)
    {
        var tag = context.Tags.Where(x => x.Id == id).Include(i => i.Items).FirstOrDefault();
        if  (tag == null)
            return NotFound();
        var items = tag.Items.Select(x=> x.Name).ToArray();
        var listTag = new TagListWithItem
        {
            Name = tag.Name,
            ItemNames = items
        };
        return Ok(listTag);
    }
    
    [HttpPut]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, TagUpdateDto dto)
    {
        var tag = context.Tags.Find(id);
        if (tag == null)
            return NotFound();
        tag.Name = dto.Name;
        context.SaveChanges();
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var tag = context.Tags.Find(id);
        if (tag == null)
            return NotFound();
        context.Tags.Remove(tag);
        context.SaveChanges();
        return NoContent();
    }
}