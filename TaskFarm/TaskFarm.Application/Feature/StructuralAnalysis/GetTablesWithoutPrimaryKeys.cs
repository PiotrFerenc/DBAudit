using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;

[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.GetTablesWithoutPrimaryKeys")]
public class GetTablesWithoutPrimaryKeys : IRequest<Unit>
{
}

public class GetTablesWithoutPrimaryKeysHandler : IRequestHandler<GetTablesWithoutPrimaryKeys, Unit>
{
    public ValueTask<Unit> Handle(GetTablesWithoutPrimaryKeys request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
