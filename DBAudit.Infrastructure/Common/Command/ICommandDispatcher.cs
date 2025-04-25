namespace DBAudit.Infrastructure.Common.Command;

public interface ICommandDispatcher
{
    Task HandleAsync<TCommand>(TCommand command) where TCommand : ICommand;
}