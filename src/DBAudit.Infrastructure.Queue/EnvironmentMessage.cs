using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record EnvironmentMessage(Guid Id) : IRequest;