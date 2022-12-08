using MainAPI.Models;
using MainAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IIdentityService,IdentityService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IdentitySettings>(builder.Configuration);
builder.Services.AddScoped<IdentityMiddleware>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue(typeof(string), "FrontUrl")!.ToString()!)
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//здесь кастомная аутентификация и авторизация через identity
app.UseMiddleware<IdentityMiddleware>();

app.UseCors("CorsPolicy");

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
