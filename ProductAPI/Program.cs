using Microsoft.EntityFrameworkCore;
using MIdentity;
using ProductAPI.Core;
using ProductAPI.Persistance;
using ProductAPI.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(p => true)
       .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddMIdentity(builder);

var app = builder.Build();

app.UseCors("CorsPolicy");
//здесь кастомная аутентификация и авторизация через identity
app.UseMiddleware<IdentityMiddleware>();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
