using LanguageExt;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

namespace DBAudit.Infrastructure.Repositories
{
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

    public class EnvironmentService(IStorage<Environment> storage, IEncryptionService encryptionService, IQueueProvider queue) : IEnvironmentService
    {
        public List<Environment> GetAll() => storage.FetchAll();

        public void Add(string id, Environment environment)
        {
            environment.ConnectionString = encryptionService.Encrypt(environment.ConnectionString);
            storage.SaveItem(id, environment);

            queue.Enqueue(new EnvironmentMessage(Guid.Parse(id)));
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

        public Option<Environment> GetById(Guid id) => storage.Find(x => x.Id == id);

        public Option<string> GetConnectionString(Guid id)
            => GetById(id)
                .Match(
                    e => encryptionService.Decrypt(e.ConnectionString),
                    () => Option<string>.None
                );
    }


    public interface IEnvironmentService
    {
        List<Environment> GetAll();
        void Add(string id, Environment environment);
        void Activate(string id);
        void Deactivate(string id);
        void ChangeName(string id, string name);
        Option<Environment> GetById(Guid id);
        Option<string> GetConnectionString(Guid id);
    }
}