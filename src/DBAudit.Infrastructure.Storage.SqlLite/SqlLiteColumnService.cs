using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.SqlLite;

public class SqlLiteColumnService(SqlLiteDbContext dbContext) : IColumnService
{
    public void Add(Column column)
    {
        dbContext.Columns.Add(column);
        dbContext.SaveChanges();
    }

    public List<Column> GetByTableId(Guid tableId) => dbContext.Columns.Where(x => x.TableId == tableId).ToList();

    public Option<Column> GetByName(Guid tableId, string name) => dbContext.Columns.Find(x => x.TableId == tableId && x.Name == name);
}