using MessagePack;

namespace DBAudit.Infrastructure.Contracts.Entities;

[MessagePackObject(AllowPrivate = true)]
public class Table
{
    [Key(0)] public Guid Id { get; set; }
    [Key(1)] public string Name { get; set; } = string.Empty;
    [Key(2)] public bool IsActive { get; set; }
    [Key(3)] public Guid DatabaseId { get; set; }
    [Key(4)] public Guid EnvironmentId { get; set; }

    public static Table Create(string name) => new Table
    {
        Id = Guid.NewGuid(),
        Name = name,
        IsActive = true
    };
}