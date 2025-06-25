using InventoryManager.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using InventoryManager.Config;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
        policy => policy.RequireRole("Administrator"));
});
builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
    {
        opt.Password.RequiredLength = 1;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequiredUniqueChars = 0;
        opt.Password.RequireDigit = false;
        opt.Password.RequiredUniqueChars = 0;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
app.MapGroup("Auth").MapIdentityApi<IdentityUser>().WithTags("Auth");
MappingConfig.RegisterMappings();


app.Run();