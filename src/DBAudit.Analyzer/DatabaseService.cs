using System.Data;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class DatabaseService : IDatabaseService
{
    public async Task<Option<T>> QuerySingleData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map)
    {
        if (connection.State is not ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        await using var command = new SqlCommand(query, connection);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            return map(reader);
        }

        return Option<T>.None;
    }

    public async Task<List<T>> QueryData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map)
    {
        if (connection.State is not ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        await using var command = new SqlCommand(query, connection);

        await using var reader = await command.ExecuteReaderAsync();
        var result = new List<T>();
        while (await reader.ReadAsync())
        {
            var item = map(reader);
            result.AddRange(item);
        }

        return result;
    }
}