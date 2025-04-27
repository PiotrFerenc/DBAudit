using System.Threading.Channels;
using DBAudit.Analyzer;
using DBAudit.Analyzer.Table.Extensions;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Common.Command;
using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Repositories;
using DBAudit.Infrastructure.SqlServer;
using Microsoft.Data.SqlClient;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Encryption:Key").Value ?? throw new InvalidOperationException("Encryption:Key not found.");
var iv = builder.Configuration.GetSection("Encryption:IV").Value ?? throw new InvalidOperationException("Encryption:IV not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));
builder.Services.AddTransient<IEnvironmentService, EnvironmentService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddTransient<IColumnService, ColumnService>();
builder.Services.AddSingleton<IStorage<Environment>>(new Storage<Environment>("environments.bin", EnvironmentMapper.MapFromString, EnvironmentMapper.MapToString));
builder.Services.AddSingleton<IStorage<Database>>(new Storage<Database>("databases.bin", DatabaseMapper.MapFromString, DatabaseMapper.MapToString));
builder.Services.AddSingleton<IStorage<Table>>(new Storage<Table>("tables.bin", TableMapper.MapFromString, TableMapper.MapToString));
builder.Services.AddSingleton<IStorage<Column>>(new Storage<Column>("column.bin", ColumnMapper.MapFromString, ColumnMapper.MapToString));

builder.Services.AddTransient<IDatabaseProvider, SqlServerProvider>();

builder.Services.AddCommandDispatcher();
builder.Services.AddTableAnalyzer();

builder.Services.AddSingleton<IQueueProvider, ChannelQueueProvider>();

builder.Services.AddHostedService<EnvironmentProcessor>();
builder.Services.AddHostedService<DatabaseProcessor>();
builder.Services.AddHostedService<TableProcessor>();
builder.Services.AddSingleton<Channel<EnvironmentMessage>>(_ => Channel.CreateUnbounded<EnvironmentMessage>(new UnboundedChannelOptions
{
    AllowSynchronousContinuations = false,
    SingleReader = true,
    SingleWriter = true
}));

builder.Services.AddSingleton<Channel<DatabaseMessage>>(_ => Channel.CreateUnbounded<DatabaseMessage>(new UnboundedChannelOptions
{
    AllowSynchronousContinuations = false,
    SingleReader = true,
    SingleWriter = true
}));

builder.Services.AddSingleton<Channel<ColumnsMessage>>(_ => Channel.CreateUnbounded<ColumnsMessage>(new UnboundedChannelOptions
{
    AllowSynchronousContinuations = false,
    SingleReader = true,
    SingleWriter = true
}));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseRouting();


app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();


internal class ChannelQueueProvider(Channel<EnvironmentMessage> envChannel, Channel<DatabaseMessage> databaseChannel, Channel<ColumnsMessage> columnChannel) : IQueueProvider
{
    public void Enqueue(EnvironmentMessage message)
    {
        envChannel.Writer.TryWrite(message);
    }

    public void Enqueue(DatabaseMessage message)
    {
        databaseChannel.Writer.TryWrite(message);
    }

    public void Enqueue(ColumnsMessage message)
    {
        columnChannel.Writer.TryWrite(message);
    }
}


public class EnvironmentProcessor(Channel<EnvironmentMessage> channel, IDatabaseProvider databaseProvider, IDatabaseService databaseService, IQueueProvider queueProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var items = await databaseProvider.GetDatabases(message.Id);


            foreach (var database in items)
            {
                if (!databaseService.Exist(message.Id, database.Name))
                {
                    database.EnvironmentId = message.Id;
                    databaseService.Add(database);
                }

                queueProvider.Enqueue(new DatabaseMessage(message.Id, database.Id));
            }
        }
    }
}

public class DatabaseProcessor(Channel<DatabaseMessage> channel, IDatabaseProvider databaseProvider, ITableService tableService, IQueueProvider queueProvider, IAnalyzerService tableAnalyzerService, ICommandDispatcher dispatcher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var tables = await databaseProvider.GetTables(message.EnvId, message.DbId);
            var cs = databaseProvider.GetConnectionString(message.EnvId, message.DbId);
            await cs.IfSomeAsync(async connectionString =>
            {
                var connection = new SqlConnection(connectionString);
                var analyzers = tableAnalyzerService.GetDatabaseAnalyzers(connection);
                foreach (var analyzer in analyzers)
                {
                    var result = await dispatcher.Send(analyzer);
                }
            });

            foreach (var table in tables)
            {
                if (tableService.Exist(message.DbId, message.EnvId)) continue;
                table.DatabaseId = message.DbId;
                table.EnvironmentId = message.EnvId;
                tableService.Add(table);
                queueProvider.Enqueue(new ColumnsMessage(message.EnvId, message.DbId, table.Id));
            }
        }
    }
}

public class TableProcessor(Channel<ColumnsMessage> channel, IDatabaseProvider databaseProvider, IColumnService columnService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var columns = await databaseProvider.GetColumns(message.EnvId, message.DbId, message.TableId);

            foreach (var column in columns)
            {
                columnService.Add(column);
            }
        }
    }
}