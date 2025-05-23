using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record UpdateEnvironment(Guid Id) : IRequest;