using MessagePack;

namespace DBAudit.Infrastructure.Contracts.Entities;

[MessagePackObject(AllowPrivate = true)]
public class MetricsDetails
{
    [Key(0)] public Guid Id { get; set; }
    [Key(1)] public string Title { get; set; } = string.Empty;
    [Key(2)] public int Value { get; set; }
    [Key(3)] public List<(string Name, string Link)> Items { get; set; } = [];
    [Key(4)] public Guid EnvironmentId { get; set; }
    [Key(5)] public Guid DatabaseId { get; set; }
    [Key(6)] public Guid TableId { get; set; }
    [Key(7)] public Guid ColumnId { get; set; }
    [Key(8)] public string Type { get; set; } = string.Empty;
}