using DBAudit.Application.Common;
using DBAudit.Infrastructure.Command.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.RegisterCommandHandlers<IApplicationMarker>();
        services.RegisterRequestHandlers<IApplicationMarker>();
    }
}