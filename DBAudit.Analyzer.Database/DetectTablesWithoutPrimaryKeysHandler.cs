using System.Data;
using DBAudit.Infrastructure.Command;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeysHandler : IRequestHandler<DetectTablesWithoutPrimaryKeys, Result<string>>
{
    public async Task<Result<string>> HandleAsync(DetectTablesWithoutPrimaryKeys request)
    {
        var connection = request.Connection;
        if (connection.State is not ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

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
        await using var command = new SqlCommand(query, connection);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var count = reader.GetInt32(0);
            return count.ToString();
        }

        return string.Empty;
    }
}