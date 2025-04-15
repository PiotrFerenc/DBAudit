using Mediator;
using Orleans.Providers;

namespace TaskFarm.Grains.Grains;

[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Grains.Grains.JobDetails")]
public class JobDetails
{
    [Id(0)] public JobStatus Status { get; set; }
    [Id(1)] public Guid Id { get; set; }
}

public enum JobStatus
{
    Processing,
    Success,
    Exception
}
[StorageProvider(ProviderName = "Storage")]
public class JobGrain(IMediator mediator) : Grain<JobDetails>, IJobGrain
{
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await ReadStateAsync();
        await base.OnActivateAsync(cancellationToken);
    }

    public async Task Execute(IRequest<Unit> request)
    {
        State.Id = this.GetPrimaryKey();
        try
        {
            State.Status = JobStatus.Processing;
            await mediator.Send(request);
            State.Status = JobStatus.Success;
        }
        catch (Exception e)
        {
            State.Status = JobStatus.Exception;
        }
        finally
        {
            await WriteStateAsync();
        }
    }

    public async Task<JobDetails> GetDetails()
    {
        await ReadStateAsync();
        return State;
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        await WriteStateAsync();
        await base.OnDeactivateAsync(reason, cancellationToken);
    }
}