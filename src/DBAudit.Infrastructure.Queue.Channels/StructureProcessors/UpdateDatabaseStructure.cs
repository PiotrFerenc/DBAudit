using System.Threading.Channels;
using DBAudit.Application.Feature;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.Contracts.Entities;
using DBAudit.Infrastructure.DatabaseProvider;
using DBAudit.Infrastructure.Storage;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace DBAudit.Infrastructure.Queue.Channels.StructureProcessors;

public class UpdateDatabaseStructure(Channel<UpdateEnvironment> channel, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            await dispatcher.Send(new UpdateStructure()
            {
                EnvId = message.Id,
            });
        }
    }
}