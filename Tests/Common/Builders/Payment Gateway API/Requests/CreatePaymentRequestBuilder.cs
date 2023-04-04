using AutoBogus;
using Bogus.DataSets;
using Common.Builders.Domain;
using PaymentGatewayAPI.Requests;
using System.Globalization;

namespace Common.Builders.Payment_Gateway_API.Requests;

public sealed class CreatePaymentRequestBuilder : AutoFaker<CreatePaymentRequest>
{
    public CreatePaymentRequestBuilder()
    {
        RuleFor(x => x.Amount, faker => faker.Finance.Amount().ToString("F2", CultureInfo.InvariantCulture));
        RuleFor(x => x.Currency, faker => faker.Finance.Currency().Code);
    }

    public CreatePaymentRequestBuilder WithAmount(string amount)
    {
        RuleFor(b => b.Amount, amount);
        return this;
    }

    public CreatePaymentRequestBuilder WithCurrency(string currency)
    {
        RuleFor(b => b.Currency, currency);
        return this;
    }
}
