using DBAudit.Analyzer.Database.Common;
using DBAudit.Infrastructure.Common.Command;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Database.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabaseAnalyzer(this IServiceCollection services)
    {

        services.RegisterHandlers<IDatabaseAnalyzerMarker>();
        // services.Scan(scan => scan
        //     .FromAssemblyOf<IDatabaseAnalyzerMarker>()
        //     .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
        //     .AsImplementedInterfaces()
        //     .WithTransientLifetime());

        services.AddSingleton<IAnalyzerService, AnalyzerService>();
    }
}