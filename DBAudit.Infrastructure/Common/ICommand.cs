using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Common;

public interface ICommand
{
}

public interface ICommand<T> : ICommand
{
}

public interface ICommandHandler<in T> where T : ICommand
{
    Task HandleAsync(T command);
}

public interface ICommandDispatcher
{
    Task Execute<TCommand>(TCommand command) where TCommand : ICommand;
}

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task Execute<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handlerType = typeof(ICommandHandler<TCommand>);
        var service = serviceProvider.GetService(handlerType);

        if (service is not ICommandHandler<TCommand> handler) throw new Exception("Handler not found");

        var task = handler.HandleAsync(command);
        return task;
    }
}

public static class CommandDispatcherExtensions
{
    public static void AddCommandDispatcher(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        services.Scan(scan => scan
            .FromAssemblyOf<ICommand>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}

public class PrintMessage : ICommand
{
    public string Message { get; set; }
}

public class PrintMessageHandler : ICommandHandler<PrintMessage>
{
    public Task HandleAsync(PrintMessage command)
    {
        Console.WriteLine(command.Message);

        return Task.CompletedTask;
    }
}