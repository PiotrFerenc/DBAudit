using MessagePack;

namespace DBAudit.Infrastructure.Data.Entities;

[MessagePackObject(AllowPrivate = true)]
public class Database
{
    [Key(0)] public Guid Id { get; set; }
    [Key(1)] public string Name { get; set; } = string.Empty;
    [Key(2)] public bool IsActive { get; set; }
    [Key(3)] public Guid EnvironmentId { get; set; }

    public static Database Create(string name) => new Database
    {
        Id = Guid.NewGuid(),
        Name = name,
        IsActive = true,
        EnvironmentId = Guid.Empty
    };
}