using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202505261023)]
public class TableMetricsMigration : Migration
{
    public override void Up()
    {
        Create.Table("TableMetrics")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable()
            .WithColumn("DatabaseId").AsGuid().NotNullable()
            .WithColumn("TableId").AsGuid().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();

    }
    
    public override void Down()
    {
        Delete.Table("TableMetrics");
    }
}