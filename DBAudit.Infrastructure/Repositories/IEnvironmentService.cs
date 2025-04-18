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
        public Environment Get(string key) => storage.Get(key);


        public List<Environment> Get() => storage.Get();

        public void Add(string key, Environment item) => storage.Add(key, item);
    }


    public class EnvironmentService(IEnvironmentStorage storage) : IEnvironmentService
    {
        public List<Data.Entities.Environment> GetAll() => storage.Get();
    }


    public interface IEnvironmentService
    {
        List<Data.Entities.Environment> GetAll();
    }
}