using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBAudit.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Entities.Environment> Environments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Entities.Environment>().ToTable("Environments").HasKey(e => e.Id);
        builder.Entity<Entities.Environment>().Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Entity<Entities.Environment>().Property(e => e.Name).IsRequired();
        builder.Entity<Entities.Environment>().Property(e => e.ConnectionString).IsRequired();

        base.OnModelCreating(builder);
    }
}