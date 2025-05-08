using LanguageExt;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Infrastructure.Storage
{
    public interface IEnvironmentService
    {
        List<Environment> GetAll();
        List<Environment> GetActive();
        void Add(Environment environment);
        void Activate(Guid id);
        void Deactivate(Guid id);
        void ChangeName(Guid id, string name);
        Option<Environment> GetById(Guid id);
        Option<SqlConnectionStringBuilder> GetConnectionString(Guid id);
    }
}