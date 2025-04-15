using Mediator;

namespace TaskFarm.Application.Feature;

public class ConsoleWriteLineHandler : IRequestHandler<ConsoleWriteLine, Unit>
{
    public ValueTask<Unit> Handle(ConsoleWriteLine request, CancellationToken cancellationToken)
    {
        if (request.Index == 2)
        {
            throw new Exception("Exception");
        }

        Console.WriteLine("done: " + request.Index);
        return Unit.ValueTask;
    }
}