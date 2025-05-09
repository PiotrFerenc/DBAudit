using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record DatabaseAnalyzerMessage(Guid EnvId) : IRequest;