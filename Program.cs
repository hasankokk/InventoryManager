using InventoryManager.Data;
using InventoryManager.Models.DTOs.Container;
using InventoryManager.Models.DTOs.Item;
using InventoryManager.Models.DTOs.Location;
using InventoryManager.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

TypeAdapterConfig<Container, ContainerListWithLocation>
    .NewConfig()
    .Map(x => x.LocationName, x => x.Location.Name);

TypeAdapterConfig<ItemCreateDto, Item>
    .NewConfig()
    .Ignore(x => x.Tags);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();