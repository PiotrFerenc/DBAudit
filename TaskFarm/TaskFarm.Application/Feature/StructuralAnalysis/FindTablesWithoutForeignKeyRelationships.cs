using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;


[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.FindTablesWithoutForeignKeyRelationships")]
public class FindTablesWithoutForeignKeyRelationships : IRequest<Unit>
{
}

public class FindTablesWithoutForeignKeyRelationshipsHandler : IRequestHandler<FindTablesWithoutForeignKeyRelationships, Unit>
{
    public ValueTask<Unit> Handle(FindTablesWithoutForeignKeyRelationships request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
