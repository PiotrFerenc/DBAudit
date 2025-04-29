using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

namespace DBAudit.Infrastructure.Extensions;

public static class StorageExtensions
{
    public static void AddBinaryStorage(this IServiceCollection services)
    {
        services.AddSingleton<IBinaryStorage<Environment>>(new BinaryStorage<Environment>($"{nameof(Environment)}.bin"));
        services.AddSingleton<IBinaryStorage<Database>>(new BinaryStorage<Database>($"{nameof(Database)}.bin"));
        services.AddSingleton<IBinaryStorage<Table>>(new BinaryStorage<Table>($"{nameof(Table)}.bin"));
        services.AddSingleton<IBinaryStorage<Column>>(new BinaryStorage<Column>($"{nameof(Column)}.bin"));
    }
}