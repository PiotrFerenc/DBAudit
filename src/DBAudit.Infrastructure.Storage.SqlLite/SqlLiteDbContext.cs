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
    public DbSet<MetricsDetails> ColumnMetrics { get; set; }
    public DbSet<MetricsDetails> DatabaseMetrics { get; set; }
    public DbSet<MetricsDetails> EnvironmentMetrics { get; set; }
    public DbSet<MetricsDetails> TableMetrics { get; set; }
}