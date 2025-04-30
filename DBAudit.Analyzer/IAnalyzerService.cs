using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface IAnalyzerService
{
    List<TableAnalyzer> GetTableAnalyzers(SqlConnection connection, string tableName);
    List<DatabaseAnalyzer> GetDatabaseAnalyzers(SqlConnection connection);
}