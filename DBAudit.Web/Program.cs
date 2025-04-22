using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Data.Entities;
using DBAudit.Infrastructure.Extensions;
using DBAudit.Infrastructure.Repositories;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var key = builder.Configuration.GetSection("Encryption:Key").Value ?? throw new InvalidOperationException("Encryption:Key not found.");
var iv = builder.Configuration.GetSection("Encryption:IV").Value ?? throw new InvalidOperationException("Encryption:IV not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// builder.Services.AddSqlServerProvider<ApplicationDbContext>(connectionString);
// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSingleton<IEncryptionService>(new EncryptionService(key, iv));
builder.Services.AddTransient<IEnvironmentService, EnvironmentService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IStorage<Environment>>(new Storage<Environment>("environments.bin", EnvironmentMapper.MapFromString, EnvironmentMapper.MapToString));
builder.Services.AddSingleton<IStorage<Database>>(new Storage<Database>("databases.bin", DatabaseMapper.MapFromString, DatabaseMapper.MapToString));


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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();
app.RunMigration();
app.Run();