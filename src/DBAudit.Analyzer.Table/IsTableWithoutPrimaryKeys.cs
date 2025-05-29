using DBAudit.Infrastructure.Contracts.Entities;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer.Table;

public class IsTableWithoutPrimaryKeys(SqlConnection connection, Environment env, Database database, Infrastructure.Contracts.Entities.Table table)
    : TableAnalyzer(connection, $"In the database '{database.Name}', the table '{table.Name}' does not have a primary key.",   env, database, table);