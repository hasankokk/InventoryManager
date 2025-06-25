namespace InventoryManager.Models.DTOs.Item;

public class ItemFilterDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ItemCount { get; set; }
    public DateOnly STT { get; set; }
    public string ContainerName { get; set; }

}