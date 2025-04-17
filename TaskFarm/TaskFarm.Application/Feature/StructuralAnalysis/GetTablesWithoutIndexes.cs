using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;


[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.GetTablesWithoutIndexes")]
public class GetTablesWithoutIndexes : IRequest<Unit>
{
}

public class GetTablesWithoutIndexesHandler : IRequestHandler<GetTablesWithoutIndexes, Unit>
{
    public ValueTask<Unit> Handle(GetTablesWithoutIndexes request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
