using DBAudit.Infrastructure.Repositories;
using LanguageExt;

namespace DBAudit.Infrastructure.Data.Entities;

public class Column
{
    public Guid Id { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid DatabaseId { get; set; }
    public Guid TableId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public interface IColumnService
{
    void Add(Column column);
    List<Column> GetByTableId(Guid tableId);
}

public class ColumnService(IStorage<Column> storage) : IColumnService
{
    public void Add(Column column) => storage.SaveItem(column.Id.ToString(), column);
    public List<Column> GetByTableId(Guid tableId) => storage.Where(x => x.TableId == tableId);
}

public static class ColumnMapper
{
    public static Func<Column, string>[] MapToString() =>
    [
        c => c.Id.ToString(),
        c => c.EnvironmentId.ToString(),
        c => c.DatabaseId.ToString(),
        c => c.TableId.ToString(),
        c => c.Type,
        c => c.Name,
        c => c.IsActive.ToString()
    ];

    public static Action<Column, string>[] MapFromString() =>
    [
        (c, b) => c.Id = Guid.Parse(b),
        (c, b) => c.EnvironmentId = Guid.Parse(b),
        (c, b) => c.DatabaseId = Guid.Parse(b),
        (c, b) => c.TableId = Guid.Parse(b),
        (c, b) => c.Type = b,
        (c, b) => c.Name = b,
        (c, b) => c.IsActive = b == "1"
    ];
}