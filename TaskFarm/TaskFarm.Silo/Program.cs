using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

var random = new Random();
var number = random.Next(1, 10);
var siloPort = random.Next(11111, 22222);
var gatewayPort = random.Next(30000, 40000);

var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices(x => x.AddMediator());


var s = host.UseOrleans(silo =>
{
    silo.AddMemoryGrainStorage("Storage");
    // silo.AddMemoryGrainStorage("PubSubStore", options => options.Name = "PubSubStore")
    // silo.AddRedisGrainStorage("Redis", options =>
    // {
    //     options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    //     {
    //         EndPoints = { "localhost" },
    //         ClientName = "testing",
    //         DefaultDatabase = 0
    //     };
    // });

    silo.UseDashboard(d => { d.Port = 5001; });
    // silo.UseRedisClustering(r => { r.ConnectionString = "localhost:6379"; });
    silo.UseLocalhostClustering();
    silo.Configure<ClusterOptions>(option =>
    {
        option.ClusterId = "TaskFarm";
        option.ServiceId = "TaskFarm";
    });
    silo.Configure<SiloOptions>(option => { option.SiloName = "Silo-" + number; });
    // silo.ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort);
});

await s.RunConsoleAsync();