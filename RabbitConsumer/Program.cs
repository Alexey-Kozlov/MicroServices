using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitConsumer;
using RabbitConsumer.Persistance;
using RabbitConsumer.Services;

var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
var config = builder.Build();
var connectionString = config["ConnectionStrings:DefaultConnection"];
using IHost host = Host.CreateDefaultBuilder(args)    
    .ConfigureServices(
    services =>
    {
        services.AddSingleton<IRabbitService, RabbitService>();
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddAutoMapper(typeof(Mapping));
    }
    ).Build();


//UpdateDb(host.Services);
GetServices(host.Services);

//static void UpdateDb(IServiceProvider hostProvider)
//{
//    using IServiceScope serviceScope = hostProvider.CreateScope();
//    IServiceProvider provider = serviceScope.ServiceProvider;
//    AppDbContext db = provider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();
//}

static void GetServices(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;
    IRabbitService logger = provider.GetRequiredService<IRabbitService>();
    logger.ConsumeMessage();
}
