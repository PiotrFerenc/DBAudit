using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202505261024)]
public class DatabaseMetricsMigration : Migration
{
    public override void Up()
    {
        Create.Table("DatabaseMetrics")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Value").AsInt32().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable()
            .WithColumn("DatabaseId").AsGuid().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();
    }
    
    public override void Down()
    {
        Delete.Table("DatabaseMetrics");
    }
}
