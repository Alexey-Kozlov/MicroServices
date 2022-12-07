using MainAPI.Models;
using MainAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
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




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    //{
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Секретный пароль")),
    //    ValidateIssuerSigningKey = true,
    //    ValidateIssuer = false,
    //    ValidateAudience = false,
    //    ValidateLifetime = true,
    //    ClockSkew = TimeSpan.Zero //убираем дефолтное окно жизни токена в 5 минут
    //};
    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        var accessToken = context.Request.Query["access_token"];
    //        if (string.IsNullOrEmpty(accessToken))
    //        {
    //            //context.Token = accessToken;


    //        }
    //        return Task.CompletedTask;
    //    }
    //};
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    //Получаем токен из identity и делаем аутентификацию
    //context.AuthenticateAsync();

    if(context.Request.Method != "OPTIONS")
    {
        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
        {
            context.Response.Redirect(builder.Configuration.GetValue(typeof(string), "IdentitySettings:IdentityUrlLogin")!.ToString()!);
            return;
        }

    }


    //var principal = new ClaimsPrincipal();

    //var result1 = await context.AuthenticateAsync("Token1");
    //if (!result1.Succeeded)
    //{
    //    context.Response.StatusCode = 401;
    //    return;
    //}

    //if (result1?.Principal != null)
    //{
    //    principal.AddIdentities(result1.Principal.Identities);
    //}

    //context.User = principal;


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
