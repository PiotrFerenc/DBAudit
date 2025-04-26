using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface ITableAnalyzerService
{
    List<TableAnalyzer> GetTableAnalyzers(SqlConnection connection, string tableName);
}

public class TableAnalyzerService : ITableAnalyzerService
{
    public List<TableAnalyzer> GetTableAnalyzers(SqlConnection connection, string tableName)
    {
        var analyzers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsSubclassOf(typeof(TableAnalyzer)))
            .Select(x => Activator.CreateInstance(x, connection, tableName) as TableAnalyzer)
            .ToList();

        return analyzers;
    }
}