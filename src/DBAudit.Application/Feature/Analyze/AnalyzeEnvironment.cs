using System.Data;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using DBAudit.Infrastructure.Storage.Metrics;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;


namespace DBAudit.Application.Feature.Analyze;

public record AnalyzeEnvironment(Guid EnvId) : IRequest;

public class AnalyzeEnvironmentHandler(ICommandDispatcher dispatcher, IDatabaseService databaseService, IEnvironmentService environmentService) : ICommandHandler<AnalyzeEnvironment>
{
    public async Task HandleAsync(AnalyzeEnvironment command)
    {
        await environmentService.GetConnectionString(command.EnvId).IfSomeAsync(async builder =>
        {
             await environmentService.GetById(command.EnvId).IfSomeAsync(async env =>
            {
                var databases = databaseService.GetAll(command.EnvId);
                foreach (var database in databases)
                {
                    builder.InitialCatalog = database.Name;
                    await dispatcher.Send(new AnalyzeDatabase(env, database, builder));
                }
            });
        });
    }
}

public record AnalyzeDatabase(Environment env, Database database, SqlConnectionStringBuilder ConnectionStringBuilder) : IRequest;

public class AnalyzeDatabaseHandler(ICommandDispatcher dispatcher, ITableService tableService) : ICommandHandler<AnalyzeDatabase>
{
    public async Task HandleAsync(AnalyzeDatabase command)
    {
        var tables = tableService.GetAll(command.database.Id);
        await using var connection = new SqlConnection(command.ConnectionStringBuilder.ToString());

        foreach (var table in tables) await dispatcher.Send(new AnalyzeTable(command.env, command.database, table, connection));

        await connection.CloseAsync();
    }
}

public record AnalyzeTable(Environment Env, Database Database, Table Table, SqlConnection connection) : IRequest;

public class AnalyzeTableHandler(ICommandDispatcher dispatcher, IColumnService columnService, ITableAnalyzerService databaseAnalyzerService) : ICommandHandler<AnalyzeTable>
{
    public async Task HandleAsync(AnalyzeTable command)
    {
        if (command.connection.State is not ConnectionState.Open) command.connection.Open();

        var analyzers = databaseAnalyzerService.GetCheckAnalyzers(command.connection, command.Env, command.Database, command.Table);
        foreach (var analyzer in analyzers) await dispatcher.Send(analyzer).IfSomeAsync(Console.WriteLine);

        var columns = columnService.GetByTableId(command.Table.Id);
        foreach (var column in columns) await dispatcher.Send(new AnalyzeColumn(command.Env, command.Database, command.Table, column, command.connection));
    }
}

public record AnalyzeColumn(Environment Env, Database Database, Table Table, Column Column, SqlConnection connection) : IRequest;

public class AnalyzeColumnHandler : ICommandHandler<AnalyzeColumn>
{
    public async Task HandleAsync(AnalyzeColumn command)
    {
        await Task.Delay(1);
    }
}