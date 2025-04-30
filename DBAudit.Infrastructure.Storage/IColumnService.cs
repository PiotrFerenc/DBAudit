using DBAudit.Infrastructure.Contracts.Entities;

namespace DBAudit.Infrastructure.Storage;

public interface IColumnService
{
    void Add(Column column);
    List<Column> GetByTableId(Guid tableId);
}