using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class CountTableWithoutPrimaryKeys : Counter
{
    public new readonly SqlConnection Connection;

    public CountTableWithoutPrimaryKeys(SqlConnection connection) : base(connection)
    {
        Connection = connection;
        Name = "Number of tables without primary key";
    }
}