using MIdentity;
using RabbitProducer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMIdentity(builder);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddScoped<IRabbitService, RabbitService>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseMiddleware<IdentityMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
