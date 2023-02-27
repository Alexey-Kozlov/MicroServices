using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MIdentity;
using OrdersAPI.Core;
using OrdersAPI.Persistance;
using OrdersAPI.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(Mapping));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(p => true)
       .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});
builder.Services.AddScoped<IOrdersRepository,OrdersRepository>();
builder.Services.Configure<OrdersPageSettings>(builder.Configuration.GetSection("OrdersPageSettings"));
builder.Services.Configure<RolesPageSetting>(builder.Configuration.GetSection("RolesPageSetting"));
builder.Services.AddControllers();
builder.Services.AddMIdentity(builder);

var app = builder.Build();

app.UseCors("CorsPolicy");
//здесь кастомная аутентификация и авторизация через identity
app.UseMiddleware<IdentityMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
CoreServiceProvider.Provider = services;

app.Run();
