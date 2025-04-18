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
        public Option<Environment> Get(string key) => storage.Get(key);
        public void Remove(string key) => storage.Remove(key);
        public List<Environment> Get() => storage.Get();
        public void Add(string key, Environment item) => storage.Add(key, item);
        public void Update(string key, Environment item) => storage.Update(key, item);
    }


    public class EnvironmentService(IEnvironmentStorage storage, IEncryptionService encryptionService) : IEnvironmentService
    {
        public List<Environment> GetAll() => storage.Get();

        public void Add(string id, Environment environment)
        {
            environment.ConnectionString = encryptionService.Encrypt(environment.ConnectionString);
            storage.Add(id, environment);
        }

        public void Activate(string id)
            => storage.Get(id).IfSome(e =>
            {
                e.IsActive = true;
                storage.Update(id, e);
            });

        public void Deactivate(string id)
            => storage.Get(id).IfSome(e =>
            {
                e.IsActive = false;
                storage.Update(id, e);
            });

        public void ChangeName(string id, string name)
            => storage.Get(id).IfSome(e =>
            {
                e.Name = name;
                storage.Update(id, e);
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