using InventoryManager.Models.DTOs.Container;
using InventoryManager.Models.DTOs.Item;
using InventoryManager.Models.Entities;
using Mapster;

namespace InventoryManager.Config;

public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

        TypeAdapterConfig<Container, ContainerListWithLocation>
            .NewConfig()
            .Map(x => x.LocationName, x => x.Location.Name);

        TypeAdapterConfig<ItemCreateDto, Item>
            .NewConfig()
            .Ignore(x => x.Tags)
            .Ignore(x => x.UserId);

        TypeAdapterConfig<ItemListDto, Item>
            .NewConfig()
            .Ignore(x => x.Tags)
            .Map(x => x.Container.Name, i => i.ContainerName);

        TypeAdapterConfig<Item, ItemListDto>
            .NewConfig()
            .Map(x => x.ContainerName, x => x.Container.Name);

        TypeAdapterConfig<ItemUpdateDto, Item>
            .NewConfig()
            .Ignore(x => x.Tags);

        TypeAdapterConfig<Item, ItemListWithUserDto>
            .NewConfig()
            .Map(x => x.Username, x => x.User.UserName);
    }
}