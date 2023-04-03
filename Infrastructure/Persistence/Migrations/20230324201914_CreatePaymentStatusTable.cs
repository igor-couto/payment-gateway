using FluentMigrator;

namespace Infrastructure.Persistence.Migrations;

[Migration(20230324201914)]
public class CreatePaymentStatusTable : Migration
{
    public override void Up()
    {
        Create.Table("payment_status")
            
            .WithColumn("id")
                .AsByte()
                .NotNullable()
                .PrimaryKey()

            .WithColumn("status")
                .AsString(128)
                .NotNullable();
    }

    public override void Down() => Delete.Table("payment_status");
}
