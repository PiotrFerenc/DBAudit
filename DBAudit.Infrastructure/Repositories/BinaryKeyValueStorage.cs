using System.Text;

namespace DBAudit.Infrastructure.Repositories;

public interface IStorage<T> where T : new()
{
    T Get(string key);
    List<T> Get();
    void Add(string key, T item);
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
        Load();
    }

    private readonly Dictionary<string, byte[]> _data = new Dictionary<string, byte[]>();

    private void Load()
    {
        using var fs = new FileStream(_path, FileMode.Open);
        using var br = new BinaryReader(fs, Encoding.UTF8);

        var count = br.ReadInt32();
        for (var i = 0; i < count; i++)
        {
            if (br.BaseStream.Position >= br.BaseStream.Length) break;

            var key = br.ReadString();
            var value = br.ReadBytes(br.ReadInt32());
            _data.Add(key, value);
        }
    }

    public List<T> Get() => _data.Select(item => Get(item.Key)).ToList();

    private void Save()
    {
        using var fs = new FileStream(_path, FileMode.Create);
        using var bw = new BinaryWriter(fs, Encoding.UTF8);

        bw.Write(_data.Count);
        foreach (var item in _data)
        {
            bw.Write(item.Key);
            bw.Write(item.Value);
        }
    }

    public T Get(string key)
    {
        var result = new T();
        if (!_data.TryGetValue(key, out var value)) return result;

        using var ms = new MemoryStream(value);
        using var br = new BinaryReader(ms, Encoding.UTF8);

        foreach (var map in _mapFromString())
        {
            var length = br.ReadInt32();
            var ba = br.ReadBytes(length);
            var v = Encoding.UTF8.GetString(ba);
            map(result, v);
        }

        return result;
    }

    public void Add(string key, T item)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8);

        foreach (var map in _mapToString())
        {
            var ba = Encoding.UTF8.GetBytes(map(item));
            bw.Write(ba.Length);
            bw.Write(ba);
        }

        var baa = ms.ToArray();
        if (_data.ContainsKey(key))
        {
            _data[key] = baa;
        }
        else
        {
            _data.Add(key, baa);
        }

        Save();
    }
}