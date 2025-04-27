using DBAudit.Infrastructure.Common.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Table;

public class DetectTablesWithoutPrimaryKeys(SqlConnection connection, string tableName) : DatabaseAnalyzer(connection)
{
}

public class DetectTablesWithoutPrimaryKeysHandler : IRequestHandler<DetectTablesWithoutPrimaryKeys, Either<string, string>>
{
    public Task<Either<string, string>> HandleAsync(DetectTablesWithoutPrimaryKeys request)
    {
        return Task.FromResult(Either<string, string>.Right("Not implemented"));
    }
}