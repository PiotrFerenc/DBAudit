using DBAudit.Infrastructure.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class DatabaseAnalyzer(SqlConnection connection) : IRequest<Either<string, string>>
{
    private readonly SqlConnection _connection = connection;
}