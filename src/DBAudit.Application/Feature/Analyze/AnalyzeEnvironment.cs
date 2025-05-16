using System.Data;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
using LanguageExt.ClassInstances.Const;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature.Analyze;

public record AnalyzeEnvironment(Guid EnvId) : IRequest;

public class AnalyzeEnvironmentHandler(ICommandDispatcher dispatcher, IDatabaseService databaseService, IEnvironmentService environmentService) : ICommandHandler<AnalyzeEnvironment>
{
    public Task HandleAsync(AnalyzeEnvironment command)
    {
        environmentService.GetConnectionString(command.EnvId).IfSome(builder =>
        {
            var databases = databaseService.GetAll(command.EnvId);
            foreach (var database in databases)
            {
                builder.InitialCatalog = database.Name;
                dispatcher.Send(new AnalyzeDatabase(command.EnvId, database.Id, builder));
            }
        });


        return Task.CompletedTask;
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