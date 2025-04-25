namespace DBAudit.Infrastructure.Common.Command;

public interface ICommandHandler<in T> where T : ICommand
{
    Task HandleAsync(T command);
}