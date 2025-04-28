using DBAudit.Infrastructure.Common.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeys(SqlConnection connection) : DatabaseAnalyzer(connection)
{
}

public class DetectTablesWithoutPrimaryKeysHandler : IRequestHandler<DetectTablesWithoutPrimaryKeys, Either<string, string>>
{
    public Task<Either<string, string>> HandleAsync(DetectTablesWithoutPrimaryKeys request)
    {
        return Task.FromResult(Either<string, string>.Right("Not implemented"));
    }
}