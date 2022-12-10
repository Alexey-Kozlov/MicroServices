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
        policy.WithOrigins(builder.Configuration.GetValue(typeof(string), "CORS_URLS")!.ToString()!.Split(new char[] { ',' }))
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddMIdentity(builder);
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
//здесь кастомная аутентификация и авторизация через identity
app.UseMiddleware<IdentityMiddleware>();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
