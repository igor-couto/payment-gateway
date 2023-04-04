using AutoBogus;
using Bogus;
using Bogus.DataSets;
using Domain.Messages;
using FluentAssertions;
using NUnit.Framework;
using PaymentExecutor.Requests;

namespace PaymentExecutor.Unit.Requests;

public class CreateAuthorizationRequestTests
{
    private Faker<ExecutePaymentMessage> _executePaymentMessageFaker;

    [SetUp]
    public void Setup()
    {
        _executePaymentMessageFaker = new AutoFaker<ExecutePaymentMessage>()
            .RuleFor(x => x.Amount, faker => faker.Finance.Amount().ToString())
            .RuleFor(x => x.CurrencyCode, faker => faker.Finance.Currency().Code)
            .RuleFor(x => x.CreditCardHolder, faker => faker.Name.FullName())
            .RuleFor(x => x.CreditCardNumber, faker => faker.Finance.CreditCardNumber())
            .RuleFor(x => x.CreditCardVerificationValue, faker => faker.Finance.CreditCardCvv())
            .RuleFor(x => x.CreditCardExpirityMonth, faker => faker.Random.Int(1, 12))
            .RuleFor(x => x.CreditCardExpirityYear, faker => faker.Random.Int(2023, 2030));
    }

    [Test]
    public void CreateAuthorizationRequest_ShouldInitializePropertiesFromExecutePaymentMessage()
    {
        // Arrange
        var executePaymentMessage = _executePaymentMessageFaker.Generate();

        // Act
        var createAuthorizationRequest = new CreateAuthorizationRequest(executePaymentMessage);

        // Assert
        createAuthorizationRequest.MerchantId.Should().Be(executePaymentMessage.MerchantId);
        createAuthorizationRequest.Amount.Should().Be(executePaymentMessage.Amount);
        createAuthorizationRequest.Currency.Should().Be(executePaymentMessage.CurrencyCode);
        createAuthorizationRequest.CreditCard.Holder.Should().Be(executePaymentMessage.CreditCardHolder);
        createAuthorizationRequest.CreditCard.CardNumber.Should().Be(executePaymentMessage.CreditCardNumber);
        createAuthorizationRequest.CreditCard.CardVerificationValue.Should().Be(executePaymentMessage.CreditCardVerificationValue);
        createAuthorizationRequest.CreditCard.ExpirityMonth.Should().Be(executePaymentMessage.CreditCardExpirityMonth);
        createAuthorizationRequest.CreditCard.ExpirityYear.Should().Be(executePaymentMessage.CreditCardExpirityYear);
    }
}