using DBAudit.Infrastructure.Queue;
using LanguageExt;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Infrastructure.Storage.Binary;

public class EnvironmentService(IDbAuditStorage<Environment> storage, IEncryptionService encryptionService, IQueueProvider queue) : IEnvironmentService
{
    public List<Environment> GetAll() => storage.FetchAll();
    public List<Environment> GetActive() => storage.Where(x => x.IsActive);

    public void Add(Environment environment)
    {
        environment.ConnectionString = encryptionService.Encrypt(environment.ConnectionString);
        storage.SaveItem(environment);

        queue.Enqueue(new EnvironmentMessage(environment.Id));
    }

    public void Activate(Guid id) => storage.Update(e => e.IsActive = true, x => x.Id == id);
    public void Deactivate(Guid id) => storage.Update(e => e.IsActive = false, x => x.Id == id);
    public void ChangeName(Guid id, string name) => storage.Update(e => e.Name = name, x => x.Id == id);

    public Option<Environment> GetById(Guid id) => storage.Find(x => x.Id == id);

    public Option<string> GetConnectionString(Guid id)
        => GetById(id)
            .Match(
                e => encryptionService.Decrypt(e.ConnectionString),
                () => Option<string>.None
            );
}