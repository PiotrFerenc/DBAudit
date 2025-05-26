using DBAudit.Infrastructure.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Environment = DBAudit.Infrastructure.Contracts.Entities.Environment;

namespace DBAudit.Infrastructure.Storage.SqlLite;

public class SqlLiteDbContext(DbContextOptions<SqlLiteDbContext> options) : DbContext(options)
{
    public DbSet<Column> Columns { get; set; }
    public DbSet<Database> Databases { get; set; }
    public DbSet<Environment> Environments { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<ColumnMetrics> ColumnMetrics { get; set; }
    public DbSet<TableMetrics> TableMetrics { get; set; }
    public DbSet<DatabaseMetrics> DatabaseMetrics { get; set; }
    public DbSet<EnvironmentMetrics> EnvironmentMetrics { get; set; }
}