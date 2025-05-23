using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202504151355)]
public class ColumnCreateTable : Migration
{
    public override void Up()
    {
        Create.Table("Columns")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("TableId").AsGuid().NotNullable()
            .WithColumn("DatabaseId").AsGuid().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable();
        //.ForeignKey("FK_Columns_Tables", "Tables", "TableId")
        //.ForeignKey("FK_Columns_Databases", "Databases", "DatabaseId")
        //.ForeignKey("FK_Columns_Environments", "Environments", "EnvironmentsId");

    }

    public override void Down()
    {
        Delete.Table("Columns");
    }
}