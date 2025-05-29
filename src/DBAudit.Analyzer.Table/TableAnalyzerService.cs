using DBAudit.Infrastructure.Contracts.Entities;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class TableAnalyzerService : ITableAnalyzerService
{
    public List<TableAnalyzer> GetCheckAnalyzers(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    {
        var analyzers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsSubclassOf(typeof(TableAnalyzer)))
            .Select(x => Activator.CreateInstance(x, connection,  env,database,table) as TableAnalyzer)
            .ToList();

        return analyzers;
    }
}