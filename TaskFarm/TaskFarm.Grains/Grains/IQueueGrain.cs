using Mediator;

namespace TaskFarm.Grains.Grains;

[Alias("TaskFarm.Grains.Grains.IQueueGrain")]
public interface IQueueGrain : IGrainWithGuidKey
{
    [Alias("DequeueTask")]
    Task<IRequest<Unit>?> DequeueTask();

    [Alias("EnqueueTask")]
    Task EnqueueTask(IRequest<Unit> taskId);

    [Alias("ProcessTasks")]
    Task ProcessTasksAsync();

    [Alias("GetDetails")]
    Task<QueueDetails> GetDetails(Guid queueId);
}