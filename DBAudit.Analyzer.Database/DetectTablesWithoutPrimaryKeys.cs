using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class CountTableWithoutPrimaryKeys(SqlConnection connection) : Counter(connection)
{
    public new readonly SqlConnection Connection = connection;
}