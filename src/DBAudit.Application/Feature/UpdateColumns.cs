using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Queue;
using DBAudit.Infrastructure.Storage;

namespace DBAudit.Application.Feature;

public class UpdateColumns(IDatabaseProvider databaseProvider, IColumnService columnService) : ICommandHandler<ColumnsMessage>
{
    public async Task HandleAsync(ColumnsMessage message)
    {
        var columns = await databaseProvider.GetColumns(message.EnvId, message.DbId, message.TableId);

        foreach (var column in columns)
        {
            columnService.GetByName(message.TableId, column.Name).IfNone(() => columnService.Add(column));
        }
    }
}