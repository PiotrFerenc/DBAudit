using DBAudit.Analyzer.Database.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.Storage;
using LanguageExt;

namespace DBAudit.Analyzer.Database;

public class TableWithoutPrimaryKeysHandler(IReportService reportService, IQueryService databaseService, ICounterService counterService) : IRequestHandler<TableWithoutPrimaryKeys, Option<int>>
{
    public async Task<Option<int>> HandleAsync(TableWithoutPrimaryKeys request)
    {
        var tables = await databaseService.QueryData(request.Connection, QueryConstants.TablesWithoutPk, reader => (reader.GetString(0), reader.GetString(1)));

        reportService.GetByDbId(request.reportId).IfSome(report =>
            {
                var counterDetails = new CounterDetails
                {
                    Title = request.Name,
                    Value = tables.Count,
                    Id = Guid.NewGuid(),
                    Items = tables.Select(x => ($"{x.Item1}.{x.Item2}", $"/database/{report.DatabaseId}")).ToList()
                };

                counterService.Add(counterDetails);
                reportService.AddCounter(report.Id, (request.Name, tables.Count.ToString(), counterDetails.Id));
            }
        );

        return tables.Count;
    }
}