using MIdentity;
using MainAPI.Services;
using MainAPI.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(p => true)
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials()
        .WithExposedHeaders("WWW-Authenticate", "Pagination");
    });
});
builder.Services.AddMIdentity(builder);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.Configure<OrdersPageSettings>(builder.Configuration.GetSection("OrdersPageSettings"));
builder.Services.Configure<RolesPageSetting>(builder.Configuration.GetSection("RolesPageSetting"));

builder.Services.AddScoped<IProducts, Products>();
builder.Services.AddScoped<ICategory, Category>();
builder.Services.AddScoped<IOrders, Orders>();

var app = builder.Build();

app.UseCors("CorsPolicy");
//здесь кастомная аутентификация и авторизация через identity
app.UseMiddleware<IdentityMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
CoreServiceProvider.Provider = services;
app.Run();
