using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;


[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.ReportUnnecessaryTextOrBlobUsage")]
public class ReportUnnecessaryTextOrBlobUsage : IRequest<Unit>
{
}

public class ReportUnnecessaryTextOrBlobUsageHandler : IRequestHandler<ReportUnnecessaryTextOrBlobUsage, Unit>
{
    public ValueTask<Unit> Handle(ReportUnnecessaryTextOrBlobUsage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

