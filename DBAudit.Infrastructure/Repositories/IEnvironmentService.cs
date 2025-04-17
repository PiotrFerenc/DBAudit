using DBAudit.Infrastructure.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Infrastructure.Repositories
{
    public class EnvironmentService(ApplicationDbContext context) : IEnvironmentService
    {
        public async Task<IEnumerable<EnvironmentDto>> GetAllAsync()
            => await context.Environments.Select(
                env => EnvironmentDto.Create(Id.Create(env.Id), new Name(env.Name), new IsActive(env.IsActive))
            ).ToListAsync();

        public async Task<IEnumerable<EnvironmentDto>> GetAllActiveAsync()
            => await context.Environments
                .Where(x => x.IsActive)
                .Select(env => EnvironmentDto.CreateActive(Id.Create(env.Id), new Name(env.Name)))
                .ToListAsync();

        public async Task<Option<EnvironmentDto>> GetByIdAsync(Guid id)
        {
            var env = await context.Environments.FindAsync(id);
            return env == null ? Option<EnvironmentDto>.None : EnvironmentDto.Create(Id.Create(env.Id), new Name(env.Name), new IsActive(env.IsActive));
        }

        public async Task CreateAsync(Name name, Name connectionString)
        {
            var newEnvironment = new Data.Entities.Environment
            {
                Id = Guid.NewGuid(),
                Name = name.ToString(),
                IsActive = true,
                ConnectionString = connectionString.ToString()
            };

            context.Environments.Add(newEnvironment);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ChangeNameAsync(Id id, Name name)
        {
            var environment = await context.Environments.FindAsync(id);
            if (environment == null)
                return false;

            environment.Name = name.ToString();

            context.Environments.Update(environment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InactivateAsync(Id id)
        {
            var environment = await context.Environments.FindAsync(id);
            if (environment == null)
                return false;

            if (!environment.IsActive) return true;

            environment.IsActive = false;

            context.Environments.Update(environment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateAsync(Id id)
        {
            var environment = await context.Environments.FindAsync(id);
            if (environment == null)
                return false;

            if (environment.IsActive) return true;

            environment.IsActive = true;

            context.Environments.Update(environment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var environment = await context.Environments.FindAsync(id);
            if (environment == null)
                return false;

            context.Environments.Remove(environment);
            await context.SaveChangesAsync();

            return true;
        }
    }

    public record Id(Guid Value)
    {
        public static Id New() => new(Guid.NewGuid());
        public static Id Create(Guid id) => new(id);
        public override string ToString() => Value.ToString();
    };

    public record Name(string Value)
    {
        public override string ToString() => Value;
    }

    public record IsActive(bool Value)
    {
        public static IsActive Active = new(true);
        public static IsActive Inactive = new(false);
    }

    public record ConnectionString(string Value)
    {
        public override string ToString() => Value;
    }


    public class EnvironmentDto
    {
        public Id Id { get; set; }
        public Name Name { get; set; }
        public IsActive IsActive { get; set; }

        public static EnvironmentDto Create(Id id, Name name, IsActive isActive) => new()
        {
            Id = id,
            Name = name,
            IsActive = isActive
        };

        public static EnvironmentDto CreateActive(Id id, Name name) => new()
        {
            Id = id,
            Name = name,
            IsActive = IsActive.Active
        };

        public static EnvironmentDto CreateInactive(Id id, Name name) => new()
        {
            Id = id,
            Name = name,
            IsActive = IsActive.Inactive
        };
    }
}

namespace DBAudit.Infrastructure.Repositories
{
    public interface IEnvironmentService
    {
        Task<IEnumerable<EnvironmentDto>> GetAllAsync();
        Task<IEnumerable<EnvironmentDto>> GetAllActiveAsync();
        Task<Option<EnvironmentDto>> GetByIdAsync(Guid id);
        Task CreateAsync(Name name, Name connectionString);
        Task<bool> ChangeNameAsync(Id id, Name name);
        Task<bool> InactivateAsync(Id id);
        Task<bool> ActivateAsync(Id id);
        Task<bool> DeleteAsync(Guid id);
    }
}