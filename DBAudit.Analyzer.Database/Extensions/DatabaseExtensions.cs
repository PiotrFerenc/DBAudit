using DBAudit.Analyzer.Database.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Database.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabaseAnalyzer(this IServiceCollection services)
    {

        services.RegisterHandlers<IDatabaseAnalyzerMarker>();
        services.AddSingleton<IAnalyzerService, AnalyzerService>();
    }
}