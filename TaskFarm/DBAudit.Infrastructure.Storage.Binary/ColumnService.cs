using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage.Binary;

public class ColumnService(IDbAuditStorage<Column> storage) : IColumnService
{
    public void Add(Column column) => storage.SaveItem(column);
    public List<Column> GetByTableId(Guid tableId) => storage.Where(x => x.TableId == tableId);
}