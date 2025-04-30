using MessagePack;

namespace DBAudit.Infrastructure.Contracts.Entities;

[MessagePackObject(AllowPrivate = true)]
public class Column
{
    [Key(0)] public Guid Id { get; set; }
    [Key(1)] public Guid EnvironmentId { get; set; }
    [Key(2)] public Guid DatabaseId { get; set; }
    [Key(3)] public Guid TableId { get; set; }
    [Key(4)] public string Type { get; set; } = string.Empty;
    [Key(5)] public string Name { get; set; } = string.Empty;
    [Key(6)] public bool IsActive { get; set; }
}