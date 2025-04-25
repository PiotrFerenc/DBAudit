namespace DBAudit.Infrastructure.Common.Command;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task HandleAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handlerType = typeof(ICommandHandler<TCommand>);
        var service = serviceProvider.GetService(handlerType);

        if (service is not ICommandHandler<TCommand> handler) throw new Exception("Handler not found");

        var task = handler.HandleAsync(command);
        return task;
    }
}