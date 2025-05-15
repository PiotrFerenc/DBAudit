using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class AnalyzerService : IAnalyzerService
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

public record TableId(Guid Value)
{
    public static TableId Empty => new(Guid.Empty);
    public static TableId Create(Guid id) => new(id);
};

public record DbId(Guid Value)
{
    public static DbId Empty => new(Guid.Empty);
    public static DbId Create(Guid id) => new(id);
};

public record EnvId(Guid Value)
{
    public static EnvId Empty => new(Guid.Empty);
    public static EnvId Create(Guid id) => new(id);
};