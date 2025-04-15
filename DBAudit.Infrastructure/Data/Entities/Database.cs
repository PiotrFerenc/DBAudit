namespace DBAudit.Infrastructure.Data.Entities;

public class Database
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid EnvironmentId { get; set; }
}