using System.ComponentModel.DataAnnotations;
using InventoryManager.Models.Entities;

namespace InventoryManager.Models.DTOs.Container;

public class ContainerCreateDto
{
    [MaxLength(100)]
    public string Name { get; set; }
    public int LocationId { get; set; }
}