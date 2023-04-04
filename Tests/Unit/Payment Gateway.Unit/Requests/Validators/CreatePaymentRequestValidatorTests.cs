using FluentValidation.TestHelper;
using NUnit.Framework;
using Common.Builders.Payment_Gateway_API.Requests;
using PaymentGatewayAPI.Requests.Validators;

namespace PaymentGatewayUnitAPI.Requests.Validators;

public class CreatePaymentRequestValidatorTests
{
    private CreatePaymentRequestValidator _validator;
    private CreatePaymentRequestBuilder _createPaymentRequestBuilder;

    [SetUp]
    public void Setup()
    {
        _createPaymentRequestBuilder = new CreatePaymentRequestBuilder();

        _validator = new CreatePaymentRequestValidator();
    }

    [Test]
    public void CreatePaymentRequestValidator_ShouldNotHaveValidationError_WithValidCreatePaymentRequest()
    {
        // Arrange
        var createPaymentRequest = _createPaymentRequestBuilder.Generate();

        // Act
        var validationResult = _validator.TestValidate(createPaymentRequest);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void CreatePaymentRequestValidator_ShouldHaveValidationError_WithInvalidCurrency(string currency)
    {
        // Arrange
        var createPaymentRequest = _createPaymentRequestBuilder
            .WithCurrency(currency)
            .Generate();

        // Act & Assert
        _validator.TestValidate(createPaymentRequest).ShouldHaveValidationErrorFor(x => x.Currency);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void CreatePaymentRequestValidator_ShouldHaveValidationError_WithInvalidAmount(string amount)
    {
        // Arrange
        var createPaymentRequest = _createPaymentRequestBuilder
            .WithAmount(amount)
            .Generate();

        // Act & Assert
        _validator.TestValidate(createPaymentRequest).ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [TestCase("100")]
    [TestCase("100.")]
    [TestCase("10.001")]
    [TestCase("aaa")]
    [TestCase("aaa.bbb")]
    [TestCase("$50")]
    [TestCase("R$50.00")]
    public void CreatePaymentRequestValidator_ShouldHaveValidationError_WithIncorrectMonetaryFormatAmount(string amount)
    {
        // Arrange
        var createPaymentRequest = _createPaymentRequestBuilder
            .WithAmount(amount)
            .Generate();

        // Act & Assert
        _validator.TestValidate(createPaymentRequest).ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [TestCase("0.00")]
    [TestCase("100.00")]
    [TestCase("9999.99")]
    [TestCase("9999.99")]
    public void CreatePaymentRequestValidator_ShouldNotHaveValidationError_WithCorrectMonetaryFormatAmount(string amount)
    {
        // Arrange
        var createPaymentRequest = _createPaymentRequestBuilder
            .WithAmount(amount)
            .Generate();

        // Act & Assert
        _validator.TestValidate(createPaymentRequest).ShouldNotHaveAnyValidationErrors();
    }
}