using Orleans.Configuration;

namespace TaskFarm.Client.Extensions;

internal static class TaskFarmExtensions
{
    public static void AddTaskFarmClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITaskQueueManager, TaskQueueManager>();
        builder.Host.UseOrleansClient((_, client) =>
        {
            // client.UseRedisClustering(r => { r.ConnectionString = "localhost:6379"; });
            client.UseLocalhostClustering();

            client.Configure<ClusterOptions>(option =>
            {
                option.ClusterId = "TaskFarm";
                option.ServiceId = "TaskFarm";
            });
        }).ConfigureLogging(logging => logging.AddConsole());
    }
}