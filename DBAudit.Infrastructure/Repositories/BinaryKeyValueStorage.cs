using System.Text;
using LanguageExt;

namespace DBAudit.Infrastructure.Repositories;

public interface IStorage<T> where T : new()
{
    Option<T> Find(string key);
    Option<T> Find(Func<T, bool> filter);
    void RemoveByKey(string key);
    List<T> FetchAll();
    void SaveItem(string key, T item);
    void UpdateItem(string key, T item);
}

public class Storage<T> : IStorage<T> where T : new()
{
    public delegate Func<T, string>[] MapToString();

    public delegate Action<T, string>[] MapFromString();

    private readonly string _path;
    private readonly MapFromString _mapFromString;
    private readonly MapToString _mapToString;

    public Storage(string path, MapFromString mapFromString, MapToString mapToString)
    {
        _path = path;
        _mapFromString = mapFromString;
        _mapToString = mapToString;

        if (!File.Exists(_path)) return;
        ReadFromFile();
    }

    private readonly Dictionary<string, byte[]> _data = new();
    private readonly Dictionary<string, T> _items = new();

    private void ReadFromFile()
    {
        using var fs = new FileStream(_path, FileMode.Open);
        using var br = new BinaryReader(fs, Encoding.UTF8);

        var count = br.ReadInt32();
        for (var i = 0; i < count; i++)
        {
            if (br.BaseStream.Position >= br.BaseStream.Length) break;

            var key = br.ReadString();
            var value = br.ReadBytes(br.ReadInt32());
            FindFromData(value).IfSome(item => _items.Add(key, item));
        }
    }

    public void RemoveByKey(string key)
    {
        _items.Remove(key);
        FlushData();
    }

    public List<T> FetchAll() => _items.Values.ToList();
    public Option<T> Find(string key) => _items.TryGetValue(key, out var item) ? item : Option<T>.None;
    public Option<T> Find(Func<T, bool> filter) => _items.Values.FirstOrDefault(filter);

    private Option<T> FindFromData(byte[] value)
    {
        var result = new T();

        using var ms = new MemoryStream(value);
        using var br = new BinaryReader(ms, Encoding.UTF8);

        foreach (var map in _mapFromString())
        {
            if (br.BaseStream.Position >= br.BaseStream.Length) break;
            var length = br.ReadInt32();
            var ba = br.ReadBytes(length);
            var v = Encoding.UTF8.GetString(ba);
            map(result, v);
        }

        return result;
    }

    public void SaveItem(string key, T item)
    {
        _items.Add(key, item);
        FlushData();
    }

    private void AddItemToData(string key, T item)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8);

        foreach (var map in _mapToString())
        {
            var m = map(item);
            var ba = Encoding.UTF8.GetBytes(m);
            bw.Write(ba.Length);
            bw.Write(ba);
        }

        var baa = ms.ToArray();
        if (!_data.TryAdd(key, baa))
        {
            _data[key] = baa;
        }
    }

    private void FlushData()
    {
        using var fs = new FileStream(_path, FileMode.Create);
        using var bw = new BinaryWriter(fs, Encoding.UTF8);
        foreach (var item in _items)
        {
            AddItemToData(item.Key, item.Value);
        }

        bw.Write(_data.Count);
        foreach (var item in _data)
        {
            bw.Write(item.Key);
            bw.Write(item.Value.Length);
            bw.Write(item.Value);
        }

        bw.Flush();
        fs.Flush();
        _data.Clear();
    }

    public void UpdateItem(string key, T item)
    {
        RemoveByKey(key);
        SaveItem(key, item);
    }
}