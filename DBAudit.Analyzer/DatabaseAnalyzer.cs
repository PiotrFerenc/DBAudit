using DBAudit.Infrastructure.Command;
using LanguageExt.Common;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class DatabaseAnalyzer(SqlConnection connection) : IRequest<Result<string>>
{
    protected SqlConnection Connection = connection;
}