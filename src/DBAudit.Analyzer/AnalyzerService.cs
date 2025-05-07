using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class AnalyzerService : IAnalyzerService
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

    public List<Counter> GetDatabaseCounters(SqlConnection connection)
    {
        var analyzers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsSubclassOf(typeof(Counter)))
            .Select(x => Activator.CreateInstance(x, connection) as Counter)
            .ToList();

        return analyzers;
    }
}