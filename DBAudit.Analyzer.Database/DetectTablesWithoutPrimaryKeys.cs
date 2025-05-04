using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeys(SqlConnection connection) : DatabaseAnalyzer(connection)
{
    public new SqlConnection Connection = connection;
}