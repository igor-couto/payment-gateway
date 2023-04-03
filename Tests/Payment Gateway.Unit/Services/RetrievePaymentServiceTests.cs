using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayAPI.Responses;
using PaymentGatewayAPI.Services;

namespace PaymentGatewayUnitAPI.Services;

public class RetrievePaymentServiceTests
{
    private RetrievePaymentService _retrievePaymentService;
    private Mock<ILogger<RetrievePaymentService>> _loggerMock;
    private DatabaseContext _databaseContext;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<RetrievePaymentService>>();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "paymentsDb")
            .Options;

        _databaseContext = new DatabaseContext(options);
        _retrievePaymentService = new RetrievePaymentService(_loggerMock.Object, _databaseContext);
    }

    [Test]
    public async Task Execute_ShouldReturnPaymentResponse_WhenPaymentExists()
    {
        // Arrange
        var payment = AutoFaker.Generate<Payment>();
        _databaseContext.Payments.Add(payment);
        await _databaseContext.SaveChangesAsync();

        // Act
        var result = await _retrievePaymentService.Execute(payment.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PaymentResponse>();
        result.Id.Should().Be(payment.Id);
        result.CheckoutId.Should().Be(payment.CheckoutId);
        result.MerchantId.Should().Be(payment.MerchantId);
        result.ShopperId.Should().Be(payment.ShopperId);
        result.Amount.Should().Be(payment.Amount);
        result.Currency.Should().Be(payment.Currency);
        result.PaymentStatus.Should().Be(payment.PaymentStatus.ToString());
        result.StatusMessage.Should().Be(payment.StatusMessage);
    }

    [Test]
    public async Task Execute_ShouldReturnNull_WhenPaymentDoesNotExist()
    {
        // Arrange
        var nonExistentPaymentId = Guid.NewGuid();

        // Act
        var result = await _retrievePaymentService.Execute(nonExistentPaymentId);

        // Assert
        result.Should().BeNull();
    }

    [TearDown]
    public void TearDown()
    {
        _databaseContext.Database.EnsureDeleted();
    }
}
