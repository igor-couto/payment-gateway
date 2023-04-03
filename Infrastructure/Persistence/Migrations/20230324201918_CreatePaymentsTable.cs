using FluentMigrator;
using Domain.Entities;

namespace Infrastructure.Persistence.Migrations;

[Migration(20230324201918)]
public class CreatePaymentsTable : Migration
{
    public override void Up()
    {
        Create.Table("payments")

            .WithColumn("id")
                .AsGuid()
                .NotNullable()
                .PrimaryKey()

            .WithColumn("checkout_id")
                .AsGuid()
                .NotNullable()

            .WithColumn("shopper_id")
                .AsGuid()
                .NotNullable()

            .WithColumn("merchant_id")
                .AsGuid()
                .NotNullable()

            .WithColumn("masked_credit_card_number")
                .AsString()
                .NotNullable()

            .WithColumn("amount")
                .AsString()
                .NotNullable()

            .WithColumn("currency")
                .AsString(3)
                .ForeignKey("currency", "alphabetic_code")
                .NotNullable()

            .WithColumn("payment_status")
                .AsByte()
                .ForeignKey("payment_status", "id")
                .WithDefaultValue((short)PaymentStatus.NotStarted)
                .NotNullable()

            .WithColumn("status_message")
                .AsString()
                .Nullable()

            .WithColumn("created_at")
                .AsDateTime()
                .NotNullable()
                .WithDefaultValue(DateTime.UtcNow)

            .WithColumn("updated_at")
                .AsDateTime()
                .Nullable();
    }

    public override void Down() => Delete.Table("payment");
}
