using DBAudit.Infrastructure.Command;

namespace DBAudit.Infrastructure.Queue;

public class AnalyzeDatabase : IRequest
{
    public Guid EnvId { get; set; }
    public Guid DbId { get; set; }
}