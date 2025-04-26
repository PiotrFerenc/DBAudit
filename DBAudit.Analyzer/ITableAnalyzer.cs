using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface ITableAnalyzer
{
    List<TableAnalyzer> GetTableAnalyzers(SqlConnection connection, string tableName);;
}