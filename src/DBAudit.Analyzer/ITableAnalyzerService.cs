using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public interface ITableAnalyzerService
{
    List<Is> GetCheckAnalyzers(SqlConnection connection, TableId tableId, EnvId envId, DbId dbId);
}