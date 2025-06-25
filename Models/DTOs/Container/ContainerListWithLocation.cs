using InventoryManager.Models.Entities;

namespace InventoryManager.Models.DTOs.Container;

public class ContainerListWithLocation
{
    public string Name { get; set; }
    public int LocationId { get; set; }
    public string LocationName { get; set; }
}