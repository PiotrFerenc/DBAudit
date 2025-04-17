using Mediator;

namespace TaskFarm.Application.Feature.StructuralAnalysis;


[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.StructuralAnalysis.ListNullableColumnsWithoutJustification")]
public class ListNullableColumnsWithoutJustification : IRequest<Unit>
{
}

public class ListNullableColumnsWithoutJustificationHandler : IRequestHandler<ListNullableColumnsWithoutJustification, Unit>
{
    public ValueTask<Unit> Handle(ListNullableColumnsWithoutJustification request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
