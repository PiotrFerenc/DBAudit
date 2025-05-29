using DBAudit.Infrastructure.Contracts.Entities;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer;

public interface ITableAnalyzerService
{
    List<TableAnalyzer> GetCheckAnalyzers(SqlConnection connection, Environment env, Database database, Table table);
}