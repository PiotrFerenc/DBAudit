using DBAudit.Infrastructure.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class TableAnalyzer(SqlConnection connection, string tableName) : IRequest<Either<string, string>>
{
}