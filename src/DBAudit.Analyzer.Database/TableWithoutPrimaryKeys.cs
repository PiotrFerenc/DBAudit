using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Database;

public class TableWithoutPrimaryKeys : Counter
{
    public new readonly SqlConnection Connection;

    public TableWithoutPrimaryKeys(SqlConnection connection, Guid reportId) : base(connection, reportId)
    {
        Connection = connection;
        Name = "Tables without primary key";
    }
}