using Common.Builders.PaymentGatewayAPI.Requests;
using FluentValidation.TestHelper;
using NUnit.Framework;
using PaymentGatewayAPI.Requests.Validators;

namespace Unit.PaymentGatewayAPI.Requests.Validators;

public class CreditCardRequestValidatorTests
{
    private CreditCardRequestValidator _validator;
    private CreditCardRequestBuilder _creditCardRequestBuilder;

    [SetUp]
    public void Setup()
    {
        _validator = new CreditCardRequestValidator();
        _creditCardRequestBuilder = new CreditCardRequestBuilder();
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void CreditCardRequestValidator_ShouldFail_WhenHolderIsEmpty(string holder)
    {
        // Arrange
        var creditCardRequest = _creditCardRequestBuilder
            .WithHolder(holder)
            .Generate();

        // Act & Assert
        _validator.TestValidate(creditCardRequest).ShouldHaveValidationErrorFor(x => x.Holder);
    }

    [Test]
    public void CreditCardRequestValidator_ShouldFail_WhenHolderIsTooLong()
    {
        // Arrange
        var creditCardRequest = _creditCardRequestBuilder
            .WithHolder(new string('a', 256))
            .Generate();

        // Act & Assert
        _validator.TestValidate(creditCardRequest).ShouldHaveValidationErrorFor(x => x.Holder);
    }

    [Test]
    [TestCase("invalid_card_number")]
    [TestCase("ABCD EFGH 1234 5678")]
    [TestCase("4050-2213-9575-7961")]
    [TestCase("5317-9845-7098-6015")]
    public void CreditCardRequestValidator_ShouldFail_WhenCardNumberIsInvalid(string cardNumber)
    {
        // Arrange
        var creditCardRequest = _creditCardRequestBuilder
            .WithCardNumber(cardNumber)
            .Generate();
        
        // Act
        var result = _validator.TestValidate(creditCardRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }

    [Test]
    [TestCase("3782 8224 6310 005")]
    [TestCase("5555555555554444")]
    [TestCase("4111111111111111")]
    [TestCase("5390187242722033")]
    [TestCase("4749-5674-0100-8392")]
    public void CreditCardRequestValidator_ShouldSucceed_WhenCardNumberIsValid(string cardNumber)
    {
        // Arrange
        var creditCardRequest = _creditCardRequestBuilder
            .WithCardNumber(cardNumber)
            .Generate();

        // Act
        var result = _validator.TestValidate(creditCardRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }
}
