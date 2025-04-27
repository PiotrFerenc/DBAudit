using DBAudit.Infrastructure.Common.Command;
using LanguageExt;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class DatabaseAnalyzer(SqlConnection connection) : IRequest<Either<string, string>>
{
}