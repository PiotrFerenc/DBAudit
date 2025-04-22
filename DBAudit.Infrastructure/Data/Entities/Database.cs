namespace DBAudit.Infrastructure.Data.Entities;

public class Database
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid EnvironmentId { get; set; }

    public static Database Create(string name) => new Database
    {
        Id = Guid.NewGuid(),
        Name = name,
        IsActive = true,
        EnvironmentId = Guid.Empty
    };
}