using InventoryManager.Data;
using InventoryManager.Models.DTOs.Item;
using InventoryManager.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Controllers;

[ApiController]
[Route("/item")]
//[Authorize]
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
        return Created("/item/{id}", dto);
    }
    [HttpGet]
    [Route("")]
    [ProducesResponseType<ItemListDto[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAll(string? search = null)
    {
        var keywords = (search ?? "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        var items = context.Items
            .Include(x => x.Container)
            .Include(x => x.Tags)
            .ToList();
        
        if (keywords.Length == 0 && search == null)
        {
            var allItems = items.Select(x => new ItemListDto
            {
                Name = x.Name,
                Description = x.Description,
                ContainerName = x.Container?.Name,
                TagName = x.Tags?.Select(t => t.Name).ToList()
            }).ToList();
            return Ok(allItems);
        }

        var filtered = items
            .Where(item =>
                keywords.Any(word =>
                    item.Name != null &&
                    item.Name.Contains(word, StringComparison.OrdinalIgnoreCase)))
            .GroupBy(x => x.Id)
            .Select(g => g.First())
            .ToList();

        var result = filtered.Select(x => new ItemListDto
        {
            Name = x.Name,
            Description = x.Description,
            ContainerName = x.Container?.Name,
            TagName = x.Tags?.Select(t => t.Name).ToList()
        }).ToList();

        return Ok(result);
    }

    [HttpGet]
    [Route("/item/filter")]
    [ProducesResponseType<ItemListDto[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetByTag(string? search = null)
    {
        var items = context.Items
            .Include(x => x.Container)
            .Include(x => x.Tags)
            .Where(x => x.Tags.Any(t => t.Name == search)).ToList();
        List<ItemFilterDto> filters = new List<ItemFilterDto>();
        foreach (var item in items)
        {
            if (item.Tags.Any(t => t.Name == search))
            {
                filters.Add(item.Adapt<ItemFilterDto>());
            }
        }

        if (filters.Count == 0)
            return NotFound("Aramanıza uyuşan hiçbir kayıt bulunmamaktadır.");
        return Ok(filters);
    }

    [HttpGet]
    [Route("/item/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBySTTFilter(bool onlyExpired = false, bool onlyApproaching = false, int limit = 0, int skip = 0)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var next7Days = today.AddDays(7);

        var items = context.Items
            .Include(x => x.Container)
            .Include(x => x.Tags)
            .ToList();

        var filtered = items.Where(x => x.STT.HasValue).ToList();
        
        //Tarihi geçmiş itemler
        if (onlyExpired)
        {
            filtered = filtered
                .Where(x => x.STT < today)
                .OrderByDescending(x => x.STT)
                .ToList();
        }
        //tarihi yaklaşanlar
        else if (onlyApproaching)
        {
            filtered = filtered
                .Where(x => x.STT >= today && x.STT <= next7Days)
                .OrderBy(x => x.STT)
                .ToList();
        }

        if (skip > 0)
            filtered = filtered.Skip(skip).ToList();
        if (limit > 0)
            filtered = filtered.Take(limit).ToList();

        var result = filtered.Select(x => new ItemListDto
        {
            Name = x.Name,
            Description = x.Description,
            ItemCount = x.ItemCount,
            STT = x.STT!.Value,
            TagName = x.Tags.Select(t => t.Name).ToList(),
            ContainerName = x.Container?.Name
        }).ToList();

        return Ok(result);
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