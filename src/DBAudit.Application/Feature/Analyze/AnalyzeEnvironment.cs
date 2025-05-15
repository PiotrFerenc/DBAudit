using System.Data;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Storage;
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
        if (connection.State is not ConnectionState.Open) await connection.OpenAsync();

        foreach (var table in tables) await dispatcher.Send(new AnalyzeTable(command.EnvId, command.DbId, table.Id, connection));
    }
}

public record AnalyzeTable(Guid EnvId, Guid DbId, Guid TableId, SqlConnection connection) : IRequest;

public class AnalyzeTableHandler(ICommandDispatcher dispatcher, IColumnService columnService, ITableAnalyzerService databaseAnalyzerService) : ICommandHandler<AnalyzeTable>
{
    public Task HandleAsync(AnalyzeTable command)
    {
        var analyzers = databaseAnalyzerService.GetCheckAnalyzers(command.connection, TableId.Create(command.TableId), EnvId.Create(command.EnvId), DbId.Create(command.DbId));
        foreach (var analyzer in analyzers)
        {
            dispatcher.Send(analyzer);
        }

        var columns = columnService.GetByTableId(command.TableId);
        foreach (var column in columns)
        {
            dispatcher.Send(new AnalyzeColumn(command.EnvId, command.DbId, command.TableId, column.Id, command.connection));
        }

        return Task.CompletedTask;
    }
}

public record AnalyzeColumn(Guid EnvId, Guid DbId, Guid TableId, Guid ColumnId, SqlConnection connection) : IRequest;

public class AnalyzeColumnHandler : ICommandHandler<AnalyzeColumn>
{
    public Task HandleAsync(AnalyzeColumn command)
    {
        return Task.CompletedTask;
    }
}