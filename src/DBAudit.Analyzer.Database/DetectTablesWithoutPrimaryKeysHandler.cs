using DBAudit.Analyzer.Database.Common;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using LanguageExt;

namespace DBAudit.Analyzer.Database;

public class CountTableWithoutPrimaryKeysHandler(IQueryService databaseService) : IRequestHandler<CountTableWithoutPrimaryKeys, Option<int>>
{
    public async Task<Option<int>> HandleAsync(CountTableWithoutPrimaryKeys request) =>
        await databaseService.QuerySingleData(request.Connection, QueryConstants.TablesWithoutPk, reader => reader.GetInt32(0));
}