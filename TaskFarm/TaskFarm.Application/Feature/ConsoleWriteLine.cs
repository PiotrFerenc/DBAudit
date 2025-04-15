using Mediator;

namespace TaskFarm.Application.Feature;

[Serializable]
[GenerateSerializer]
[Alias("TaskFarm.Application.Feature.ConsoleWriteLine")]
public class ConsoleWriteLine : IRequest<Unit>
{
    [Id(0)] public int Index { get; set; }
}