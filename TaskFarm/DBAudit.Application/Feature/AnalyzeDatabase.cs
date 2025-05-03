using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature;

public class AnalyzeDatabase : IRequest
{
    public Guid EnvId { get; set; }
    public Guid DbId { get; set; }
}

public class AnalyzeDatabaseHandler(IDatabaseProvider databaseProvider, IAnalyzerService analyzerService, ICommandDispatcher dispatcher) : ICommandHandler<AnalyzeDatabase>
{
    public async Task HandleAsync(AnalyzeDatabase message)
    {
        var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
        await cs.IfSomeAsync(async connectionString =>
        {
            var connection = new SqlConnection(connectionString);
            var analyzers = analyzerService.GetDatabaseAnalyzers(connection);
            foreach (var analyzer in analyzers)
            {
                var result = await dispatcher.Send(analyzer);
            }
        });
    }
}