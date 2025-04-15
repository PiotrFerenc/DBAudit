using Mediator;
using TaskFarm.Client.Extensions;
using TaskFarm.Grains.Grains;

namespace TaskFarm.Client;

internal class TaskQueueManager : ITaskQueueManager
{
    private readonly IQueueGrain _queue;
    private readonly Guid _id;
    private readonly IClusterClient _client;

    public TaskQueueManager(IClusterClient client)
    {
        _id = Guid.NewGuid();
        _client = client;
        _queue = _client.GetGrain<IQueueGrain>(_id);
    }

    public async Task EnqueueTask(IRequest<Unit> request) => await _queue.EnqueueTask(request);

    public async Task<Guid> ProcessTasksAsync()
    {
        await _queue.ProcessTasksAsync();
        return _id;
    }

    public async Task<JobDetails> GetJobDetailsAsync(Guid jobId)
    {
        var details = _client.GetGrain<IJobGrain>(jobId);
        return await details.GetDetails();
    }

    public async Task<QueueDetails> GetQueueDetailsAsync(Guid queueId)
    {
        var details = _client.GetGrain<IQueueGrain>(queueId);
        return await details.GetDetails(queueId);
    }
}