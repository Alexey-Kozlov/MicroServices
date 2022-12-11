using MIdentity;
using MainAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue(typeof(string), "FrontUrl")!.ToString()!)
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});
builder.Services.AddMIdentity(builder);
builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddScoped<IProducts, Products>();
builder.Services.AddScoped<ICategory, Category>();

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
