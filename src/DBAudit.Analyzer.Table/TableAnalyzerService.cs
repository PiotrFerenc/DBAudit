using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Table;

public class TableAnalyzerService : ITableAnalyzerService
{
    public List<Is> GetCheckAnalyzers(SqlConnection connection, TableId tableId, EnvId envId, DbId dbId)
    {
        var analyzers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsSubclassOf(typeof(Is)))
            .Select(x => Activator.CreateInstance(x, connection, tableId.Value, envId.Value, dbId.Value) as Is)
            .ToList();

        return analyzers;
    }
}