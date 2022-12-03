using Microsoft.IdentityModel.Logging;
using System.Net;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(15))
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration.GetValue(typeof(string), "IdentityUrl")!.ToString();
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = "MyMS";
    options.ClientSecret = "MySecretKey";
    options.ResponseType = "code";
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    options.Scope.Add("MyMS");
    options.SaveTokens = true;
    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect("/");
            context.HandleResponse();
            return Task.FromResult(0);
        }
    };

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
