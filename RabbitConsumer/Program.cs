using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Persistance;
using RabbitConsumer.Services;
using RabbitConsumer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHostedService<RabbitConsumerService>();
builder.Services.AddAutoMapper(typeof(Mapping));
//сервис для синхронизации при записи в БД
//builder.Services.AddSingleton<ISaveDb, SaveDb>();

var app = builder.Build();
app.Run();
