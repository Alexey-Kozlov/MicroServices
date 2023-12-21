using MIdentity;
using MainAPI.Services;
using MainAPI.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue(typeof(string), "FrontUrl")!.ToString()!)
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "IdentityServer.Demo.Api",
            Version = "v1",
        });
    c.CustomSchemaIds(x => x.FullName);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
});
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
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
CoreServiceProvider.Provider = services;
app.Run();
