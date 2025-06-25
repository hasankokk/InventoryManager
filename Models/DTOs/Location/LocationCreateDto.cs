using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models.DTOs.Location;

public class LocationCreateDto
{
    [MaxLength(100)]
    public string Name { get; set; }
}