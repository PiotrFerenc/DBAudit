using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface IDbAuditStorage<T>
{
    Option<T> Find(Func<T, bool> filter);
    List<T> Where(Func<T, bool> filter);
    void RemoveByKey(Func<T, bool> filter);
    List<T> FetchAll();
    void SaveItem(T item);
    void Update(Action<T> item, Func<T, bool> filter);
}