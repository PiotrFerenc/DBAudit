using DBAudit.Infrastructure.Command;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class Is(SqlConnection connection, string name, Guid tableId, Guid envId, Guid dbId) : IRequest
{
    public SqlConnection connection { get; } = connection;
    public string name { get; } = name;
    public Guid tableId { get; } = tableId;
    public Guid envId { get; } = envId;
    public Guid dbId { get; } = dbId;
}