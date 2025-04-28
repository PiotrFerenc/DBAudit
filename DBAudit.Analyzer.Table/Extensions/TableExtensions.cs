using DBAudit.Analyzer.Table.Common;
using DBAudit.Infrastructure.Common.Command;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Table.Extensions;

public static class TableExtensions
{
    public static void AddTableAnalyzer(this IServiceCollection services)
    {
        services.RegisterHandlers<ITableAnalyzerMarker>();

        services.AddSingleton<IAnalyzerService, AnalyzerService>();
    }
}