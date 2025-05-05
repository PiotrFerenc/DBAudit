using DBAudit.Infrastructure.Command;
using LanguageExt.Common;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeysHandler(IDatabaseService databaseService) : IRequestHandler<DetectTablesWithoutPrimaryKeys, Result<string>>
{
    public async Task<Result<string>> HandleAsync(DetectTablesWithoutPrimaryKeys request)
    {
        const string query = """

                             SELECT COUNT(*) AS TablesWithoutPK
                             FROM sys.tables t
                             WHERE NOT EXISTS (
                                 SELECT 1
                                 FROM sys.indexes i
                                 WHERE i.object_id = t.object_id
                                   AND i.is_primary_key = 1
                             );
                             """;

        var count = await databaseService.QuerySingleData(request.Connection, query, reader => reader.GetInt32(0));

        return count.Match(c => c.ToString(), () => string.Empty);
    }
}