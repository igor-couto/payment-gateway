using FluentMigrator;
using Domain.Entities;

namespace Infrastructure.Persistence.Migrations;

[Migration(20230324201915)]
public class SeedPaymentStatusTable : Migration
{
    public override void Up()
    {
        Insert.IntoTable("payment_status")
            .Row(new
            {
                id = (short) PaymentStatus.NotStarted,
                status = PaymentStatus.NotStarted.ToString()
            })
            .Row(new
            {
                id = (short)PaymentStatus.Authorized,
                status = PaymentStatus.Authorized.ToString()
            })
            .Row(new
            {
                id = (short)PaymentStatus.Success,
                status = PaymentStatus.Success.ToString()
            })
            .Row(new
            {
                id = (short)PaymentStatus.Failed,
                status = PaymentStatus.Failed.ToString()
            });
    }

    public override void Down()
    => Delete.FromTable("payment_status")
        .Row(new { id = PaymentStatus.NotStarted })
        .Row(new { id = PaymentStatus.Authorized })
        .Row(new { id = PaymentStatus.Success })
        .Row(new { id = PaymentStatus.Failed });
}