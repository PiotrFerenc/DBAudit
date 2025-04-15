using Mediator;

namespace TaskFarm.Grains.Grains;

[Alias("TaskFarm.Grains.Grains.IJobGrain")]
public interface IJobGrain : IGrainWithGuidKey
{
    [Alias("Execute")]
    Task Execute(IRequest<Unit> request);

    [Alias("GetDetails")]
    Task<JobDetails> GetDetails();
}