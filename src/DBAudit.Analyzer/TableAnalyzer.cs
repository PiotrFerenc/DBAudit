using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Analyzer;

public class TableAnalyzer(SqlConnection connection,string name, Environment env, Database database,Table table) : IRequest<Option<string>>
{
    public SqlConnection connection { get; } = connection;
    public string name { get; } = name;
    public Environment env { get; } = env;
    public Database database { get; } = database;
    public Table table { get; } = table;
}