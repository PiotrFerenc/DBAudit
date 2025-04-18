namespace DBAudit.Infrastructure.Data.Entities;

public class Environment
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
}