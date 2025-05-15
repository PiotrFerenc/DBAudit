using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer.Table;

public class IsTableWithoutPrimaryKeys(SqlConnection connection, Guid tableId, Guid envId, Guid dbId)
    : Is(connection, "Tables without primary key", tableId, envId, dbId);