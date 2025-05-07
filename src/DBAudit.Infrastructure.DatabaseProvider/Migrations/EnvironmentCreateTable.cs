using FluentMigrator;

namespace DBAudit.Infrastructure.Data.Migrations;

[Migration(202504151353)]
public class EnvironmentCreateTable : Migration
{
    public override void Up()
    {
        Create.Table("Environments")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("ConnectionString").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Environments");
    }
}