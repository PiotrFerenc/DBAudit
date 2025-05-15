// using DBAudit.Infrastructure.Command;
// using DBAudit.Infrastructure.DatabaseProvider;
// using DBAudit.Infrastructure.Queue;
// using DBAudit.Infrastructure.Storage;
// using Microsoft.Data.SqlClient;
//
// namespace DBAudit.Application.Feature;
//
// public class AnalyzeDatabaseHandler(IDatabaseProvider databaseProvider, IQueueProvider queryProvider, IReportService reportService, IDatabaseService databaseService, ITableService tableService, IColumnService columnService, IEnvironmentService environmentService) : ICommandHandler<AnalyzeDatabase>
// {
//     public Task HandleAsync(AnalyzeDatabase message)
//     {
//         databaseProvider.GetConnectionString(message.EnvId, message.DbId)
//             .IfSome(connectionString =>
//             {
//                 databaseService.GetById(message.DbId).IfSome(database =>
//                 {
//                     var cs = new SqlConnectionStringBuilder(connectionString)
//                     {
//                         InitialCatalog = database.Name
//                     };
//                     var connection = new SqlConnection(cs.ToString());
//                     
//                     queryProvider.Enqueue(new CounterMetricMessage(connection, Guid.NewGuid()));
//                     
//                     var tables = tableService.GetAll(database.Id);
//
//                     foreach (var table in tables)
//                     {
//                         var columns = columnService.GetByTableId(table.Id);
//
//                         foreach (var column in columns)
//                         {
//                         }
//                     }
//                 });
//             });
//
//         return Task.CompletedTask;
//     }
// }