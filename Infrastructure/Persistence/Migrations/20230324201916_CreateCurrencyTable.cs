using FluentMigrator;

namespace Infrastructure.Persistence.Migrations;

[Migration(20230324201916)]
public class CreateCurrencyTable : Migration
{
    public override void Up()
    {
        Create.Table("currency")

            .WithColumn("alphabetic_code")
                .AsString(3)
                .NotNullable()
                .PrimaryKey()

            .WithColumn("name")
                .AsString()
                .NotNullable();
    }

    public override void Down() => Delete.Table("currency");
}
