using DBAudit.Infrastructure.Common.Command;
using Microsoft.Extensions.DependencyInjection;

namespace DBAudit.Analyzer.Table.Extensions;

public static class TableExtensions
{
    public static void AddTableAnalyzer(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TableAnalyzer>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());


        services.Scan(scan => scan
            .FromAssemblyOf<TableAnalyzer>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.AddSingleton<IAnalyzerService, AnalyzerService>();
    }
}