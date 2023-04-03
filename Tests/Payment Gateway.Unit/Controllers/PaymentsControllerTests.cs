using AutoBogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayAPI.Controllers;
using PaymentGatewayAPI.Exceptions;
using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Responses;
using PaymentGatewayAPI.Services.Interfaces;

namespace PaymentGatewayUnitAPI.Controllers;

internal class PaymentsControllerTests
{
    private PaymentsController _paymentsController;
    private Mock<ILogger<PaymentsController>> _loggerMock;
    private Mock<ICreatePaymentService> _createPaymentServiceMock;
    private Mock<IRetrievePaymentService> _retrievePaymentServiceMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<PaymentsController>>();
        _createPaymentServiceMock = new Mock<ICreatePaymentService>();
        _retrievePaymentServiceMock = new Mock<IRetrievePaymentService>();
        _paymentsController = new PaymentsController(_loggerMock.Object);
    }

    [Test]
    public async Task Post_ShouldReturnCreated_WhenPaymentIsSuccessfullyCreated()
    {
        // Arrange
        var createPaymentRequest = AutoFaker.Generate<CreatePaymentRequest>();
        var createdPaymentResponse = AutoFaker.Generate<PaymentResponse>();
        _createPaymentServiceMock.Setup(service => service.Execute(createPaymentRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdPaymentResponse);

        // Act
        var result = await _paymentsController.Post(createPaymentRequest, _createPaymentServiceMock.Object);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = (CreatedAtActionResult)result;
        createdAtResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdAtResult.Value.Should().BeEquivalentTo(createdPaymentResponse);
    }

    [Test]
    public async Task Post_ShouldReturnConflict_WhenPaymentAlreadyExists()
    {
        // Arrange
        var createPaymentRequest = AutoFaker.Generate<CreatePaymentRequest>();
        _createPaymentServiceMock
            .Setup(service => service.Execute(createPaymentRequest, It.IsAny<CancellationToken>()))
            .Throws(new PaymentAlreadyExistsException());

        // Act
        var result = await _paymentsController.Post(createPaymentRequest, _createPaymentServiceMock.Object);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = (ConflictObjectResult)result;
        conflictResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        conflictResult.Value.Should().Be("Could not create payment because it already exists.");
    }

    [Test]
    public async Task Get_ShouldReturnOk_WhenPaymentIsFound()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = AutoFaker.Generate<PaymentResponse>();
        _retrievePaymentServiceMock
            .Setup(service => service.Execute(paymentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        // Act
        var result = await _paymentsController.Get(paymentId, _retrievePaymentServiceMock.Object);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(payment);
    }

    [Test]
    public async Task Get_ShouldReturnNotFound_WhenPaymentIsNotFound()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        _retrievePaymentServiceMock
            .Setup(service => service.Execute(paymentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PaymentResponse) null!);

        // Act
        var result = await _paymentsController.Get(paymentId, _retrievePaymentServiceMock.Object);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"Requested payment with id {paymentId} not found");
    }
}
