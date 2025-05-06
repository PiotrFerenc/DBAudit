using DBAudit.Infrastructure.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class Counter(SqlConnection connection) : IRequest<Option<int>>
{
    protected SqlConnection Connection = connection;
    public string Name { get; set; }
}