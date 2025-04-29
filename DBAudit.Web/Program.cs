using DBAudit.Analyzer.Database.Extensions;
using DBAudit.Analyzer.Table.Extensions;
using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Common.Command;
using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Extensions;
using DBAudit.Infrastructure.Queue.Channels;
using DBAudit.Infrastructure.Repositories;
using DBAudit.Infrastructure.SqlServer;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Encryption:Key").Value ?? throw new InvalidOperationException("Encryption:Key not found.");
var iv = builder.Configuration.GetSection("Encryption:IV").Value ?? throw new InvalidOperationException("Encryption:IV not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));
builder.Services.AddTransient<IEnvironmentService, EnvironmentService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddTransient<IColumnService, ColumnService>();

builder.Services.AddBinaryStorage();

builder.Services.AddTransient<IDatabaseProvider, SqlServerProvider>();
builder.Services.AddHostedService<EnvironmentProcessor>();

builder.Services.AddCommandDispatcher();

builder.Services.AddDatabaseAnalyzer();
builder.Services.AddTableAnalyzer();



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