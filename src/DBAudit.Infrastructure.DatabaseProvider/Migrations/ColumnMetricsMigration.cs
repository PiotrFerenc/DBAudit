using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202505261022)]
public class ColumnMetricsMigration : Migration
{
    public override void Up()
    {
        Create.Table("Metrics")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("Key").AsString().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();
    }
    
    public override void Down()
    {
        Delete.Table("Metrics");
    }
}