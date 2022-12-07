using MainAPI.Models;
using MainAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IIdentityService,IdentityService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IdentitySettings>(builder.Configuration);

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

app.Use(async (context, next) =>
{
    //Здесь кастомная аутентификация - через отдельный сервис аутентификации
    //Получаем токен из identity и делаем аутентификацию
    if(context.Request.Method != "OPTIONS")
    {
        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
        {
            var retUrl = builder.Configuration.GetValue(typeof(string), "IdentitySettings:IdentityUrlLogin")!.ToString();
            if(context.Request.GetDisplayUrl().Contains(builder.Configuration.GetValue(typeof(string), "FrontUrl")!.ToString()!))
            {
                retUrl += "?ReturnUrl=" + context.Request.Path;
            }
            context.Response.Redirect(retUrl!);
            return;
        }
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var identityService = services.GetRequiredService<IIdentityService>();

        var principal = await identityService.GetPrincipal(token!);
        if(principal != null)
        { 
            //валидный токен, получен пользователь
            context.User = principal;
        }
        else
        {
            //ошибка с токеном - не авторизован
            throw new HttpRequestException("Невалидный токен", new Exception(), HttpStatusCode.Unauthorized);
        }
    }
    await next();
});
app.UseCors("CorsPolicy");

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
