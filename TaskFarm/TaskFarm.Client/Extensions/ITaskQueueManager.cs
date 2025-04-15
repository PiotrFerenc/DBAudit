using Mediator;
using TaskFarm.Grains.Grains;

namespace TaskFarm.Client.Extensions;

internal interface ITaskQueueManager
{
    Task EnqueueTask(IRequest<Unit> request);
    Task<Guid> ProcessTasksAsync();
    Task<JobDetails> GetJobDetailsAsync(Guid jobId);
    Task<QueueDetails> GetQueueDetailsAsync(Guid queueId);
}