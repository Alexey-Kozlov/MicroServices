using CategoryAPI.Core;
using CategoryAPI.Persistance;
using CategoryAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MIdentity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
