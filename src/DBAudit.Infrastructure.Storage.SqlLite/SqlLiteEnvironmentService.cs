using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Infrastructure.Storage.SqlLite;

public class SqlLiteEnvironmentService(SqlLiteDbContext dbContext, IEncryptionService encryptionService) : IEnvironmentService
{
    public List<Environment> GetAll() => dbContext.Environments.ToList();

    public List<Environment> GetActive() => dbContext.Environments.Where(x => x.IsActive).ToList();

    public void Add(Environment environment)
    {
        environment.ConnectionString = encryptionService.Encrypt(environment.ConnectionString);
        dbContext.Environments.Add(environment);
        dbContext.SaveChanges();
    }

    public void Activate(Guid id)
    {
        var environment = dbContext.Environments.Find(id);
        if (environment == null) return;
        environment.IsActive = true;
        dbContext.SaveChanges();
    }

    public void Deactivate(Guid id)
    {
        var environment = dbContext.Environments.Find(id);
        if (environment == null) return;
        environment.IsActive = false;
        dbContext.SaveChanges();
    }

    public void ChangeName(Guid id, string name)
    {
        var environment = dbContext.Environments.Find(id);
        if (environment == null) return;
        environment.Name = name;
        dbContext.SaveChanges();
    }

    public Option<Environment> GetById(Guid id) => dbContext.Environments.Find(id);

    public Option<SqlConnectionStringBuilder> GetConnectionString(Guid id)
        => GetById(id)
            .Match(
                e => new SqlConnectionStringBuilder(encryptionService.Decrypt(e.ConnectionString)),
                () => Option<SqlConnectionStringBuilder>.None
            );
}