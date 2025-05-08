using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure;

public interface IQueryService
{
    Task<Option<T>> QuerySingleData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map);
    Task<List<T>> QueryData<T>(SqlConnection connection, string query, Func<SqlDataReader, T> map);
}