using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Environment = DBAudit.Infrastructure.Data.Entities.Environment;

namespace DBAudit.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Environment> Environments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Environment>().Property(e => e.Name).IsRequired();
        builder.Entity<Environment>().Property(e => e.ConnectionString).IsRequired();

        base.OnModelCreating(builder);
    }
}