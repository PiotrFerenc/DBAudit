using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;

[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.IdentifyUndocumentedTablesAndColumns")]
public class IdentifyUndocumentedTablesAndColumns : IRequest<Unit>
{
}

public class IdentifyUndocumentedTablesAndColumnsHandler : IRequestHandler<IdentifyUndocumentedTablesAndColumns, Unit>
{
    public ValueTask<Unit> Handle(IdentifyUndocumentedTablesAndColumns request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}