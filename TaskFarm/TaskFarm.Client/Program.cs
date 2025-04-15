using TaskFarm.Application.Feature;
using TaskFarm.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddTaskFarmClient();

var app = builder.Build();

app.MapGet("/run", async (ITaskQueueManager client) =>
{
    for (int i = 0; i < 3; i++)
    {
        await client.EnqueueTask(new ConsoleWriteLine()
        {
            Index = i
        });
    }

    var queueId = await client.ProcessTasksAsync();
    return new { queueId };
});

app.MapGet("/run/{id:guid}", async (Guid id, ITaskQueueManager client) => await client.GetQueueDetailsAsync(id));

app.Run();