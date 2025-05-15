using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure.Command.Extensions;
using DBAudit.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Table.Extensions;

public static class TableExtensions
{
    public static void AddTableAnalyzer(this IServiceCollection services)
    {
        services.RegisterRequestHandlers<ITableAnalyzerMarker>();

        services.AddSingleton<IAnalyzerService, AnalyzerService>();
    }
}