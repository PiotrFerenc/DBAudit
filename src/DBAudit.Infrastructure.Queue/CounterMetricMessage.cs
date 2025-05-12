using DBAudit.Infrastructure.Command;
using Microsoft.Data.SqlClient;

namespace DBAudit.Infrastructure.Queue;

public record CounterMetricMessage(SqlConnection connection, Guid reportId) : IRequest;