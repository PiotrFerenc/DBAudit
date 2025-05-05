using DBAudit.Analyzer.Database.Common;
using DBAudit.Infrastructure.Command;
using LanguageExt.Common;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeysHandler(IDatabaseService databaseService) : IRequestHandler<DetectTablesWithoutPrimaryKeys, Result<string>>
{
    public async Task<Result<string>> HandleAsync(DetectTablesWithoutPrimaryKeys request) =>
        await databaseService.QuerySingleData(request.Connection, QueryConstants.TablesWithoutPk, reader => reader.GetInt32(0)).Match(c => c.ToString(), () => string.Empty);
}