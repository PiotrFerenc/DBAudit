using DBAudit.Analyzer.Database.Extensions;
using DBAudit.Analyzer.Table.Extensions;
using DBAudit.Application.Extensions;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Command;
using DBAudit.Infrastructure.DatabaseProvider.SqlServer.Extensions;
using DBAudit.Infrastructure.Extensions;
using DBAudit.Infrastructure.Queue.Channels;
using DBAudit.Infrastructure.Queue.Channels.Extensions;
using DBAudit.Infrastructure.Queue.Channels.StructureProcessors;
using DBAudit.Infrastructure.Storage.SqlLite.Common;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Encryption:Key").Value ?? throw new InvalidOperationException("Encryption:Key not found.");
var iv = builder.Configuration.GetSection("Encryption:IV").Value ?? throw new InvalidOperationException("Encryption:IV not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));

builder.Services.AddAddSqlLiteStorage(builder.Configuration);

builder.Services.AddSqlServerProvider();

builder.Services.AddCommandDispatcher();
builder.Services.AddApplication();

builder.Services.AddDatabaseAnalyzer();
builder.Services.AddTableAnalyzer();
builder.Services.AddChanelQueue();

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
app.RunMigration();
app.Run();