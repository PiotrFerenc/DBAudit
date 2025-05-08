using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;

namespace DBAudit.Application.Feature;

public class AnalyzeDatabase : IRequest
{
    public Guid EnvId { get; set; }
    public Guid DbId { get; set; }
}

public class AnalyzeDatabaseHandler(IDatabaseProvider databaseProvider, IQueueProvider queryProvider, IReportService reportService) : ICommandHandler<AnalyzeDatabase>
{
    public Task HandleAsync(AnalyzeDatabase message)
    {
        var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
        cs.IfSome(connectionString =>
        {
            var connection = new SqlConnection(connectionString);
            reportService.Remove(message.DbId);
            reportService.Add(new ReportView
            {
                DatabaseId = message.DbId,
                Title = "123",
                Links = [],
                Counters = []
            });
            
            queryProvider.Enqueue(new CounterMetricMessage(connection, message.DbId));
            
        });

        return Task.CompletedTask;
    }
}