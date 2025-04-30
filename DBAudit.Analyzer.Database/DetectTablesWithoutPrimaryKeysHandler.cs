using DBAudit.Infrastructure.Command;
using LanguageExt;

namespace DBAudit.Analyzer.Database;

public class DetectTablesWithoutPrimaryKeysHandler : IRequestHandler<DetectTablesWithoutPrimaryKeys, Either<string, string>>
{
    public Task<Either<string, string>> HandleAsync(DetectTablesWithoutPrimaryKeys request)
    {
        return Task.FromResult(Either<string, string>.Right("Not implemented"));
    }
}