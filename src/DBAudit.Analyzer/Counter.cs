using DBAudit.Infrastructure.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class Counter(SqlConnection connection, Guid reportId) : IRequest<Option<int>>
{
    public Guid reportId { get; } = reportId;
    protected SqlConnection Connection = connection;
    public string Name { get; set; }
}