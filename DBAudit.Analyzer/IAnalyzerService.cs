using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface IAnalyzerService
{
    List<TableAnalyzer> GetTableAnalyzers(SqlConnection connection, string tableName);
    List<Counter> GetDatabaseAnalyzers(SqlConnection connection);
}