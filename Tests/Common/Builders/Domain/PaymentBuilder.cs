using System.Globalization;
using AutoBogus;
using Domain.Entities;
using PaymentGatewayAPI.Requests.Validators;

namespace Common.Builders.Domain;

public sealed class PaymentBuilder : AutoFaker<Payment>
{
    public PaymentBuilder()
    {

        RuleFor(x => x.Amount, faker => faker.Random.Decimal(1, 100000).ToString("0.00", CultureInfo.InvariantCulture));
        RuleFor(x => x.Currency, faker => faker.PickRandom(CreatePaymentRequestValidator.CurrencyCodes));
        RuleFor(x => x.PaymentStatus, PaymentStatus.NotStarted);
        RuleFor(x => x.UpdatedAt, (DateTime?) null);
    }

    public PaymentBuilder WithAmount(string amount)
    {
        RuleFor(b => b.Amount, amount);
        return this;
    }

    public PaymentBuilder Authorized()
    {
        RuleFor(b => b.PaymentStatus, PaymentStatus.Authorized);
        return this;
    }

    public PaymentBuilder Finished()
    {
        RuleFor(b => b.PaymentStatus, PaymentStatus.Success);
        return this;
    }
    public PaymentBuilder Failed()
    {
        RuleFor(b => b.PaymentStatus, PaymentStatus.Failed);
        return this;
    }
}
