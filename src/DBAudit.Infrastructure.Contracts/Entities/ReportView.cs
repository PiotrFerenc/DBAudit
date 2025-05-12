using MessagePack;

namespace DBAudit.Infrastructure.Contracts.Entities;

[MessagePackObject(AllowPrivate = true)]
public class ReportView
{
    [Key(0)] public Guid DatabaseId { get; set; }
    [Key(1)] public string Title { get; set; }
    [Key(2)] public List<(string Title, string Link)> Links { get; set; }

    [Key(3)] public List<(string Title, string Value, Guid Id)> Counters { get; set; }

    [Key(4)] public Guid EnvId { get; set; }
    [Key(5)] public Guid Id { get; set; }

    public ReportView()
    {
        Links = new List<(string, string)>();
        Counters = new List<(string, string, Guid)>();
    }
}