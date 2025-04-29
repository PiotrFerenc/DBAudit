using DBAudit.Infrastructure.Repositories;
using LanguageExt;
using MessagePack;

namespace DBAudit.Infrastructure.Data.Entities;

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

public interface IColumnService
{
    void Add(Column column);
    List<Column> GetByTableId(Guid tableId);
}

public class ColumnService(IBinaryStorage<Column> storage) : IColumnService
{
    public void Add(Column column) => storage.SaveItem(column);
    public List<Column> GetByTableId(Guid tableId) => storage.Where(x => x.TableId == tableId);
}
