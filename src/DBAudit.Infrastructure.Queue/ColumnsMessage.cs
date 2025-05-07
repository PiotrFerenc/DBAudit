using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public record ColumnsMessage(Guid EnvId, Guid DbId, Guid TableId) : IRequest;