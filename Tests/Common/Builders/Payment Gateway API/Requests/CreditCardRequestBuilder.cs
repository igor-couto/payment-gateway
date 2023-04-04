using AutoBogus;
using PaymentGatewayAPI.Requests;

namespace Common.Builders.PaymentGatewayAPI.Requests;

public sealed class CreditCardRequestBuilder : AutoFaker<CreditCardRequest>
{
    public CreditCardRequestBuilder()
    {
        RuleFor(x => x.Holder, faker => faker.Person.FirstName);
        RuleFor(x => x.CardNumber, faker => faker.Finance.CreditCardNumber());
        RuleFor(x => x.CardVerificationValue, faker => faker.Finance.CreditCardCvv());
        RuleFor(x => x.ExpirityMonth, faker => faker.Date.Future().Month);
        RuleFor(x => x.ExpirityYear, faker => faker.Date.Future().Year);
    }

    public CreditCardRequestBuilder WithHolder(string holder)
    {
        RuleFor(x => x.Holder, holder);
        return this;
    }

    public CreditCardRequestBuilder WithCardNumber(string cardNumber)
    {
        RuleFor(x => x.CardNumber, cardNumber);
        return this;
    }

    public CreditCardRequestBuilder WithCardVerificationValue(string cardVerificationValue)
    {
        RuleFor(x => x.CardVerificationValue, cardVerificationValue);
        return this;
    }

    public CreditCardRequestBuilder WithExpirityMonth(int expirityMonth)
    {
        RuleFor(x => x.ExpirityMonth, expirityMonth);
        return this;
    }

    public CreditCardRequestBuilder WithExpirityYear(int expirityYear)
    {
        RuleFor(x => x.ExpirityYear, expirityYear);
        return this;
    }
}
