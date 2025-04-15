using Mediator;
using Orleans.Providers;

namespace TaskFarm.Grains.Grains;

[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Grains.Grains.QueueDetails")]
public class QueueDetails
{
    [Id(0)] public List<JobDetails> Jobs { get; set; } = [];
}
[StorageProvider(ProviderName = "Storage")]
public class QueueGrain : Grain<QueueDetails>, IQueueGrain
{
    private readonly Queue<IRequest<Unit>> _taskQueue = new();

    public Task EnqueueTask(IRequest<Unit> task)
    {
        _taskQueue.Enqueue(task);

        return Task.CompletedTask;
    }

    public Task<IRequest<Unit>?> DequeueTask()
    {
        return _taskQueue.Count != 0 ? Task.FromResult<IRequest<Unit>?>(_taskQueue.Dequeue()) : Task.FromResult<IRequest<Unit>?>(null);
    }

    public async Task ProcessTasksAsync()
    {
        while (_taskQueue.Count != 0)
        {
            var taskId = Guid.NewGuid();
            var taskGrain = GrainFactory.GetGrain<IJobGrain>(taskId);
            var task = _taskQueue.Dequeue();
            await taskGrain.Execute(task);
            State.Jobs.Add(await taskGrain.GetDetails());
        }

        await WriteStateAsync();
    }

    public async Task<QueueDetails> GetDetails(Guid queueId)
    {
        await ReadStateAsync();
        return State;
    }
}