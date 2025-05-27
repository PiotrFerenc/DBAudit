using System.Data;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature.Analyze;

public record AnalyzeEnvironment(Guid EnvId) : IRequest;

public class AnalyzeEnvironmentHandler(ICommandDispatcher dispatcher, IDatabaseService databaseService, IEnvironmentService environmentService) : ICommandHandler<AnalyzeEnvironment>
{
    public async Task HandleAsync(AnalyzeEnvironment command)
    {
        await environmentService.GetConnectionString(command.EnvId).IfSomeAsync(async builder =>
        {
            var databases = databaseService.GetAll(command.EnvId);
            foreach (var database in databases)
            {
                builder.InitialCatalog = database.Name;
                await dispatcher.Send(new AnalyzeDatabase(command.EnvId, database.Id, builder));
            }

            await dispatcher.Send(new GenerateMetrics(command.EnvId));
        });
    }
}

public record AnalyzeDatabase(Guid EnvId, Guid DbId, SqlConnectionStringBuilder ConnectionStringBuilder) : IRequest;

public class AnalyzeDatabaseHandler(ICommandDispatcher dispatcher, ITableService tableService) : ICommandHandler<AnalyzeDatabase>
{
    public async Task HandleAsync(AnalyzeDatabase command)
    {
        var tables = tableService.GetAll(command.DbId);
        await using var connection = new SqlConnection(command.ConnectionStringBuilder.ToString());

        foreach (var table in tables) await dispatcher.Send(new AnalyzeTable(command.EnvId, command.DbId, table.Id, connection));

        await connection.CloseAsync();
    }
}

public record AnalyzeTable(Guid EnvId, Guid DbId, Guid TableId, SqlConnection connection) : IRequest;

public class AnalyzeTableHandler(ICommandDispatcher dispatcher, IColumnService columnService, ITableAnalyzerService databaseAnalyzerService) : ICommandHandler<AnalyzeTable>
{
    public async Task HandleAsync(AnalyzeTable command)
    {
        if (command.connection.State is not ConnectionState.Open) command.connection.Open();

        var analyzers = databaseAnalyzerService.GetCheckAnalyzers(command.connection, TableId.Create(command.TableId), EnvId.Create(command.EnvId), DbId.Create(command.DbId));
        foreach (var analyzer in analyzers) await dispatcher.Send(analyzer).IfSomeAsync(Console.WriteLine);

        var columns = columnService.GetByTableId(command.TableId);
        foreach (var column in columns) await dispatcher.Send(new AnalyzeColumn(command.EnvId, command.DbId, command.TableId, column.Id, command.connection));
    }
}

public record AnalyzeColumn(Guid EnvId, Guid DbId, Guid TableId, Guid ColumnId, SqlConnection connection) : IRequest;

public class AnalyzeColumnHandler : ICommandHandler<AnalyzeColumn>
{
    public async Task HandleAsync(AnalyzeColumn command)
    {
        await Task.Delay(1);
    }
}

public record GenerateMetrics(Guid EnvId) : IRequest;

public class GenerateMetricsHandler(IColumnMetricsService metricsService, ITableService tableService, IDatabaseService databaseService, IEnvironmentService environmentService, IDatabaseMetricsService databaseMetricsService) : ICommandHandler<GenerateMetrics>
{
    public async Task HandleAsync(GenerateMetrics command)
    {
        var datebases = databaseService.GetAll(command.EnvId);
        foreach (var database in datebases)
        {
            var tableMetrics =await tableService.CountMetrics(command.EnvId, database.Id);

            foreach (var metric in tableMetrics)
            {
                databaseMetricsService.Add(DatabaseMetrics.Create(metric.Title,metric.Value, command.EnvId, database.Id ,metric.Type));
            }
        }
            
        // var metrics = metricsService.GetAllForEnv(command.EnvId);
        // var tables = tableService.GetAllByEnvId(command.EnvId);
        //     // get ze srodowiska i wszystkie tabele w nim 
        // foreach (var table in tables)
        // {
        //     var columnMetrics = metrics.Where(x => x.ColumnId != Guid.Empty && x.TableId == table.Id).ToList();
        //
        //     var result = MetricsGenerator.For(columnMetrics, metric => new MetricsDetails
        //     {
        //         Id = Guid.NewGuid(),
        //         Title = metric.Key,
        //         Value = metric.Value,
        //         Items = [],
        //         EnvironmentId = table.EnvironmentId,
        //         DatabaseId = table.DatabaseId,
        //         TableId = table.Id,
        //         ColumnId = Guid.Empty,
        //         Type = metric.Key
        //     });
        //
        //     foreach (var metric in result)
        //     {
        //         metricsService.Add(metric);
        //     }
        // }
        // metrics = metricsService.GetAllForEnv(command.EnvId);
        // var databases = databaseService.GetAll(command.EnvId);
        //
        // foreach (var database in databases)
        // {
        //     var tableMetrics = metrics.Where(x => x.ColumnId == Guid.Empty && x.TableId != Guid.Empty && x.EnvironmentId == command.EnvId).ToList();
        //     var result = MetricsGenerator.For(tableMetrics, metric => new MetricsDetails
        //     {
        //         Id = Guid.NewGuid(),
        //         Title = metric.Key,
        //         Value = metric.Value,
        //         Items = [],
        //         EnvironmentId = database.EnvironmentId,
        //         DatabaseId = database.Id,
        //         TableId = Guid.Empty,
        //         ColumnId = Guid.Empty,
        //         Type = metric.Key
        //     });
        //     foreach (var metric in result)
        //     {
        //         metricsService.Add(metric);
        //     }
        // }
        //
        // environmentService.GetById(command.EnvId).IfSome(e =>
        // {
        //     metrics = metricsService.GetAllForEnv(command.EnvId);
        //     var databaseMetrics  = metrics.Where(x => x.ColumnId == Guid.Empty && x.TableId == Guid.Empty && x.EnvironmentId == command.EnvId).ToList();
        //     var result = MetricsGenerator.For(databaseMetrics, metric => new MetricsDetails
        //     {
        //         Id = Guid.NewGuid(),
        //         Title = metric.Key,
        //         Value = metric.Value,
        //         Items = [],
        //         EnvironmentId = e.Id,
        //         DatabaseId = Guid.Empty,
        //         TableId = Guid.Empty,
        //         ColumnId = Guid.Empty,
        //         Type = metric.Key
        //     });
        //     foreach (var metric in result)
        //     {
        //         metricsService.Add(metric);
        //     }
        // });
        
    }
}

public static class MetricsGenerator
{
    
    /*
    SELECT Type, SUM(Value) as Value
    FROM ColumnMetrics
    GROUP BY Type
    */
    public static List<TableMetrics> For(List<ColumnMetrics> columnMetrics, Func<KeyValuePair<string, int>, TableMetrics> map)
        => columnMetrics.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Sum(y => y.Value))
            .Select(map).ToList();
}