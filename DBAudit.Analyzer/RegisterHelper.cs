using DBAudit.Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer;

public static class RegisterHelper
{
    public static void RegisterHandlers<TMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}