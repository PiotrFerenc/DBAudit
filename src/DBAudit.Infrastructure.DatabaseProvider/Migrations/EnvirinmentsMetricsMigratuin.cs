using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;
[Migration(202505261025)]
public class EnvironmentsMetricsMigration : Migration
{
    public override void Up()
    {
        Create.Table("EnvironmentMetrics")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Value").AsInt32().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().NotNullable();
    }
    
    public override void Down()
    {
        Delete.Table("EnvironmentMetrics");
    }
}