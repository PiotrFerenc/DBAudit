using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface IColumnService
{
    void Add(Column column);
    List<Column> GetByTableId(Guid tableId);
    Option<Column> GetByName(Guid tableId, string name);
}