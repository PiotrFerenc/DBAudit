using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class ColumnService(IDbAuditStorage<Column> storage) : IColumnService
{
    public void Add(Column column) => storage.SaveItem(column);
    public List<Column> GetByTableId(Guid tableId) => storage.Where(x => x.TableId == tableId);
    public Option<Column> GetByName(Guid tableId, string name) => storage.Find(x => x.TableId == tableId && x.Name == name);
}