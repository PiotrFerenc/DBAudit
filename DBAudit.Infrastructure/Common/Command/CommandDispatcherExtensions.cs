using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Common.Command;

public static class CommandDispatcherExtensions
{
    public static void AddCommandDispatcher(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        services.Scan(scan => scan
            .FromAssemblyOf<IRequest>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        
        services.Scan(scan => scan
            .FromAssemblyOf<IRequest>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        
    }
}