using MessagePack;

namespace DBAudit.Infrastructure.Contracts.Entities;

[MessagePackObject(AllowPrivate = true)]
public class CounterDetails
{
    [Key(0)] public string Title { get; set; } = string.Empty;
    [Key(1)] public int Value { get; set; }
    [Key(2)] public List<(string Name, string Link)> Items { get; set; }
}