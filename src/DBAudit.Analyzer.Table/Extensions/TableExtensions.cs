using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure.Command.Extensions;
using DBAudit.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Table.Extensions;

public static class TableExtensions
{
    public static void AddTableAnalyzer(this IServiceCollection services)
    {
        services.RegisterCommandHandlers<ITableAnalyzerMarker>();
        services.RegisterRequestHandlers<ITableAnalyzerMarker>();
        services.AddSingleton<ITableAnalyzerService, TableAnalyzerService>();
    }
}