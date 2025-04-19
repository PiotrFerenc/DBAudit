
using LanguageExt;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

namespace DBAudit.Infrastructure.Repositories
{
    public interface IEnvironmentStorage : IStorage<Environment>
    {
    }

    public static class EnvironmentMapper
    {
        public static Func<Environment, string>[] MapToString() =>
        [
            p => p.Id.ToString(),
            p => p.Name,
            p => p.IsActive ? "1" : "0",
            p => p.ConnectionString,
        ];

        public static Action<Environment, string>[] MapFromString() =>
        [
            (p, b) => p.Id = Guid.Parse(b),
            (p, b) => p.Name = b,
            (p, b) => p.IsActive = b == "1",
            (p, b) => p.ConnectionString = b,
        ];
    }

    public class EnvironmentStorage(IStorage<Environment> storage) : IEnvironmentStorage
    {
        public Option<Environment> Find(string key) => storage.Find(key);
        public Option<Environment> Find(Func<Environment, bool> filter) => storage.Find(filter);
        public void RemoveByKey(string key) => storage.RemoveByKey(key);
        public List<Environment> FetchAll() => storage.FetchAll();
        public void SaveItem(string key, Environment item) => storage.SaveItem(key, item);
        public void UpdateItem(string key, Environment item) => storage.UpdateItem(key, item);
        public void UpdateMany(Action<Environment> item, Func<Environment, bool> filter)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Action<Environment> item, Func<Environment, bool> filter)
        {
            throw new NotImplementedException();
        }
    }


    public class EnvironmentService(IEnvironmentStorage storage, IEncryptionService encryptionService) : IEnvironmentService
    {
        public List<Environment> GetAll() => storage.FetchAll();

        public void Add(string id, Environment environment)
        {
            environment.ConnectionString = encryptionService.Encrypt(environment.ConnectionString);
            storage.SaveItem(id, environment);
        }

        public void Activate(string id)
            => storage.Find(id).IfSome(e =>
            {
                e.IsActive = true;
                storage.UpdateItem(id, e);
            });

        public void Deactivate(string id)
            => storage.Find(id).IfSome(e =>
            {
                e.IsActive = false;
                storage.UpdateItem(id, e);
            });

        public void ChangeName(string id, string name)
            => storage.Find(id).IfSome(e =>
            {
                e.Name = name;
                storage.UpdateItem(id, e);
            });
    }


    public interface IEnvironmentService
    {
        List<Environment> GetAll();
        void Add(string id, Environment environment);
        void Activate(string id);
        void Deactivate(string id);
        void ChangeName(string id, string name);
    }
}