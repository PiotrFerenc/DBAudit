namespace DBAudit.Infrastructure.Common.Command;

public class PrintMessageHandler : ICommandHandler<PrintMessage>
{
    public Task HandleAsync(PrintMessage command)
    {
        Console.WriteLine(command.Message);

        return Task.CompletedTask;
    }
}