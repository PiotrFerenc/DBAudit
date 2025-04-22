using System.Threading.Channels;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Repositories;
using DBAudit.Infrastructure.SqlServer;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Encryption:Key").Value ?? throw new InvalidOperationException("Encryption:Key not found.");
var iv = builder.Configuration.GetSection("Encryption:IV").Value ?? throw new InvalidOperationException("Encryption:IV not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));
builder.Services.AddTransient<IEnvironmentService, EnvironmentService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IStorage<Environment>>(new Storage<Environment>("environments.bin", EnvironmentMapper.MapFromString, EnvironmentMapper.MapToString));
builder.Services.AddSingleton<IStorage<Database>>(new Storage<Database>("databases.bin", DatabaseMapper.MapFromString, DatabaseMapper.MapToString));

builder.Services.AddTransient<IDatabaseProvider, SqlServerProvider>();


builder.Services.AddHostedService<EnvironmentProcessor>();
builder.Services.AddSingleton<IQueueProvider, ChannelQueueProvider>();
builder.Services.AddSingleton<Channel<EnvironmentMessage>>(_ => Channel.CreateUnbounded<EnvironmentMessage>(new UnboundedChannelOptions
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


internal class ChannelQueueProvider(Channel<EnvironmentMessage> channel) : IQueueProvider
{
    public void Enqueue(EnvironmentMessage message)
    {
        channel.Writer.TryWrite(message);
    }
}


public class EnvironmentProcessor(Channel<EnvironmentMessage> channel, IDatabaseProvider databaseProvider, IDatabaseService databaseService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);
            var items = await databaseProvider.GetDatabases(message.Id);

            foreach (var database in items)
            {
                database.EnvironmentId = message.Id;
                databaseService.Add(database);
            }
        }
    }
}