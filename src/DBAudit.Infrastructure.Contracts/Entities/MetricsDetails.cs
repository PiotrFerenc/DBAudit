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
    [Key(9)] public DateTime CreatedAt { get; set; }

    public static MetricsDetails CreateColumnMetrics(
        string title,
        int value,
        List<(string Name, string Link)> items,
        Guid environmentId,
        Guid databaseId,
        Guid tableId,
        Guid columnId,
        string type
    ) => new()
    {
        Id = Guid.NewGuid(),
        Title = title,
        Value = value,
        Items = items,
        EnvironmentId = environmentId,
        DatabaseId = databaseId,
        TableId = tableId,
        ColumnId = columnId,
        Type = type,
        CreatedAt = DateTime.UtcNow
    };

    public static MetricsDetails CreateTableMetrics(
        string title,
        int value,
        Guid environmentId,
        Guid databaseId,
        Guid tableId,
        string type
    ) => new()
    {
        Id = Guid.NewGuid(),
        Title = title,
        Value = value,
        EnvironmentId = environmentId,
        DatabaseId = databaseId,
        TableId = tableId,
        ColumnId = Guid.Empty,
        Type = type,
        CreatedAt = DateTime.UtcNow
    };

    public static MetricsDetails CreateDatabaseMetrics(
        string title,
        int value,
        Guid environmentId,
        Guid databaseId,
        string type
    ) => new()
    {
        Id = Guid.NewGuid(),
        Title = title,
        Value = value,
        EnvironmentId = environmentId,
        DatabaseId = databaseId,
        TableId = Guid.Empty,
        ColumnId = Guid.Empty,
        Type = type,
        CreatedAt = DateTime.UtcNow
    };

    public static MetricsDetails CreateEnvMetrics(
        string title,
        int value,
        Guid environmentId,
        string type
    ) => new()
    {
        Id = Guid.NewGuid(),
        Title = title,
        Value = value,
        EnvironmentId = environmentId,
        DatabaseId = Guid.Empty,
        TableId = Guid.Empty,
        ColumnId = Guid.Empty,
        Type = type,
        CreatedAt = DateTime.UtcNow
    };
}