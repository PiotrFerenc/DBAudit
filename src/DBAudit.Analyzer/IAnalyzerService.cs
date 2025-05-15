using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface IAnalyzerService
{
    List<Is> GetCheckAnalyzers(SqlConnection connection, TableId tableId, EnvId envId, DbId dbId);
}