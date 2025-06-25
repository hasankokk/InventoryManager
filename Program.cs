using System.ComponentModel;
using InventoryManager.Data;
using InventoryManager.Models.DTOs.Container;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

//TypeAdapterConfig<Container, ContainerListWithLocation>
//    .NewConfig()
//    //.Map(x => x.LocationName, c => c.Location.Name);
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