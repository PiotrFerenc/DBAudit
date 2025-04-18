using DBAudit.Infrastructure.Data.Entities;

namespace DBAudit.Infrastructure.Repositories;

public interface IDatabaseService
{
    void Add(Database database);
    void Activate(string id);
    void Deactivate(string id);
    void Deactivate(Guid id);
    void ChangeName(string id, string name);
    List<Database> GetAll();
    Database GetById(string id);
}

public class DatabaseService : IDatabaseService
{
    private readonly IStorage<Database> _storage;

    public DatabaseService(IStorage<Database> storage)
    {
        _storage = storage;
    }

    public void Add(Database database) => _storage.SaveItem(database.Id.ToString(), database);

    public void Activate(string id) => _storage.Find(id).IfSome(d =>
    {
        d.IsActive = true;
        _storage.UpdateItem(id, d);
    });

    public void Deactivate(string id) => _storage.Find(id).IfSome(d =>
    {
        d.IsActive = false;
        _storage.UpdateItem(id, d);
    });

    public void Deactivate(Guid id) => _storage.UpdateMany(d => d.IsActive = false, d => d.EnvironmentId == id);
    public void ChangeName(string id, string name) => _storage.UpdateItem(d => d.Name = name, d => d.Id == Guid.Parse(id));
    public List<Database> GetAll() => _storage.FetchAll();

    /// Retrieves a database entity using the specified identifier.
    /// <param name="id">
    /// The unique identifier of the database entity to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Database"/> object representing the retrieved entity.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if the database entity with the specified identifier is not found.
    /// </exception>
    public Database GetById(string id) => _storage.Find(id).IfNone(() => throw new Exception("Database not found"));
}