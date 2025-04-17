using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;


[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.IdentifyTablesWithExcessiveColumnCounts")]
public class IdentifyTablesWithExcessiveColumnCounts : IRequest<Unit>
{
}

public class IdentifyTablesWithExcessiveColumnCountsHandler : IRequestHandler<IdentifyTablesWithExcessiveColumnCounts, Unit>
{
    public ValueTask<Unit> Handle(IdentifyTablesWithExcessiveColumnCounts request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
