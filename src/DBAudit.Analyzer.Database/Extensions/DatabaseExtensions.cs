using DBAudit.Analyzer.Database.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Database.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabaseAnalyzer(this IServiceCollection services)
    {
        services.RegisterRequestHandlers<IDatabaseAnalyzerMarker>();
        services.AddSingleton<IAnalyzerService, AnalyzerService>();
        services.AddSingleton<IQueryService, DatabaseService>();
    }
}