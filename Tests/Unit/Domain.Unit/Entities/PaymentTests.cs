using Common.Builders.Domain;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Unit.Domain.Entities;

public class PaymentTests
{

    private PaymentBuilder _paymentBuilder;

    [SetUp]
    public void SetUp() 
    {
        _paymentBuilder = new PaymentBuilder();
    }

    [Test]
    public void Constructor_SetsProperties()
    {
        // Arrange
        var checkoutId = Guid.NewGuid();
        var shopperId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var amount = "100.00";
        var currency = "USD";
        var creditCardNumber = "1234567890123456";

        // Act
        var payment = new Payment(checkoutId, shopperId, merchantId, amount, currency, creditCardNumber);

        // Assert
        payment.CheckoutId.Should().Be(checkoutId);
        payment.ShopperId.Should().Be(shopperId);
        payment.MerchantId.Should().Be(merchantId);
        payment.Amount.Should().Be(amount);
        payment.Currency.Should().Be(currency);
        payment.MaskedCreditCardNumber.Should().Be("************3456");
        payment.PaymentStatus.Should().Be(PaymentStatus.NotStarted);
        payment.StatusMessage.Should().BeNull();
        payment.CreatedAt.Should().NotBe(null);
        payment.UpdatedAt.Should().BeNull();
    }

    [Test]
    public void Authorize_WhenNotStarted_ChangesStatusToAuthorized()
    {
        // Arrange
        var payment = _paymentBuilder.Generate();

        // Act
        payment.Authorize();

        // Assert
        payment.PaymentStatus.Should().Be(PaymentStatus.Authorized);
        payment.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public void Authorize_WhenAlreadyAuthorized_ThrowsException()
    {
        // Arrange
        var payment = _paymentBuilder
            .Authorized()
            .Generate();

        // Act + Assert
        payment.Invoking(p => p.Authorize()).Should().Throw<PaymentChangeToAuthorizedException>();
    }

    [Test]
    public void Finish_WhenAuthorized_ChangesStatusToSuccess()
    {
        // Arrange
        var payment = _paymentBuilder
            .Authorized()
            .Generate();

        // Act
        payment.Finish();

        // Assert
        payment.PaymentStatus.Should().Be(PaymentStatus.Success);
        payment.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public void Finish_WhenNotAuthorized_ThrowsException()
    {
        // Arrange
        var payment = _paymentBuilder
            .RuleFor(p => p.PaymentStatus, f => f.PickRandom(PaymentStatus.Failed, PaymentStatus.NotStarted))
            .Generate();

        // Act + Assert
        payment.Invoking(p => p.Finish()).Should().Throw<PaymentChangeToFinishedException>();
    }

    [Test]
    public void Fail_ChangesStatusToFailedAndSetsStatusMessage()
    {
        // Arrange
        var payment = _paymentBuilder.Generate();

        var failReason = "Insufficient funds";

        // Act
        payment.Fail(failReason);

        // Assert
        payment.PaymentStatus.Should().Be(PaymentStatus.Failed);
        payment.StatusMessage.Should().Be(failReason);
        payment.UpdatedAt.Should().NotBeNull();
    }

    [Test]
    public void CreateMaskedCreditCardNumber_ReturnsMaskedString()
    {
        // Arrange
        // Arrange
        var checkoutId = Guid.NewGuid();
        var shopperId = Guid.NewGuid();
        var merchantId = Guid.NewGuid();
        var amount = "100.00";
        var currency = "USD";
        var creditCardNumber = "1234 5678 9012 3456";

        // Act
        var payment = new Payment(checkoutId, shopperId, merchantId, amount, currency, creditCardNumber);

        // Assert
        payment.MaskedCreditCardNumber.Should().Be("**** **** **** 3456");
    }
}