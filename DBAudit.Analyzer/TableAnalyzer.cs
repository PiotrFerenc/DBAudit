using DBAudit.Infrastructure.Common.Command;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Data.SqlClient;

namespace DBAudit.Analyzer;

public class TableAnalyzer(SqlConnection connection, string tableName) : IRequest<Either<string, string>>
{
}