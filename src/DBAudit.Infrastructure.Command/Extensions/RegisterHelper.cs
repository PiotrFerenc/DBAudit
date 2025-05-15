using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Infrastructure.Command.Extensions;

public static class RegisterHelper
{
    public static void RegisterRequestHandlers<TMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    public static void RegisterCommandHandlers<TMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}