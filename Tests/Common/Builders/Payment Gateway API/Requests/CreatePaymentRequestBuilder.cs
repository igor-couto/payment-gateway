using AutoBogus;
using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Requests.Validators;
using System.Globalization;

namespace Common.Builders.Payment_Gateway_API.Requests;

public sealed class CreatePaymentRequestBuilder : AutoFaker<CreatePaymentRequest>
{
    public CreatePaymentRequestBuilder()
    {
        RuleFor(x => x.Amount, faker => faker.Finance.Amount().ToString("F2", CultureInfo.InvariantCulture));
        RuleFor(x => x.Currency, faker => faker.PickRandom(CreatePaymentRequestValidator.CurrencyCodes));
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
