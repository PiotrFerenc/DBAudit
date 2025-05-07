using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class BinaryStorage<T> : IDbAuditStorage<T>
{
    private readonly string _path;
    private readonly List<T> _items;

    public BinaryStorage(string path)
    {
        _path = path;
        _items = new List<T>();
        Load();
    }

    private void Load()
    {
        if (!File.Exists(_path)) return;

        using var fs = new FileStream(_path, FileMode.Open);
        _items.Clear();
        _items.AddRange(MessagePack.MessagePackSerializer.Deserialize<List<T>>(fs));
    }

    private void Save()
    {
        using var fs = new FileStream(_path, FileMode.Create);
        var binaryData = MessagePack.MessagePackSerializer.Serialize(_items);
        fs.Write(binaryData, 0, binaryData.Length);
    }

    public Option<T> Find(Func<T, bool> filter) => _items.Find(filter);

    public List<T> Where(Func<T, bool> filter) => FetchAll().Where(filter).ToList();

    public void RemoveByKey(Func<T, bool> filter) =>
        _items.Find(filter).IfSome(x =>
        {
            _items.Remove(x);
            Save();
        });

    public List<T> FetchAll() => _items.ToList();

    public void SaveItem(T item)
    {
        _items.Add(item);
        Save();
    }

    public void Update(Action<T> item, Func<T, bool> filter)
    {
        FetchAll().Where(filter).ToList().ForEach(item);
        Save();
    }
}
