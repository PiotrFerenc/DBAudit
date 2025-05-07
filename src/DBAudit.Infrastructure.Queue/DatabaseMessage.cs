using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record DatabaseMessage(Guid EnvId, Guid DbId) : IRequest;