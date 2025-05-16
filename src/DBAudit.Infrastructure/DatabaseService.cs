using System.Data;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure;

public class DatabaseService : IQueryService
{
    public async Task<Option<T>> QuerySingleData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map)
    {
        var wasInitiallyClosed = connection.State is not ConnectionState.Open;
        try
        {
            if (wasInitiallyClosed)
            {
                await connection.OpenAsync();
            }
            
            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                return map(reader);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (wasInitiallyClosed && connection.State is ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }

        return Option<T>.None;
    }

    public async Task<List<T>> QueryData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map)
    {
        var wasInitiallyClosed = connection.State is not ConnectionState.Open;
        try
        {
            if (wasInitiallyClosed)
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
        finally
        {
            if (wasInitiallyClosed && connection.State is ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }
}