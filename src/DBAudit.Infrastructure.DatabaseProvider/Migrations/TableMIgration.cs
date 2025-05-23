using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202504151421)]
public class TableMigration : Migration
{
    public override void Up()
    {
        Create.Table("Tables")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("DatabaseId").AsGuid()//.ForeignKey("FK_Table_Database", "Database", "Id")
            .WithColumn("EnvironmentId").AsGuid().NotNullable();
        //        .ForeignKey("FK_Table_Environment", "Environment", "Id");
    }

    public override void Down()
    {
        Delete.Table("Table");
    }
}
