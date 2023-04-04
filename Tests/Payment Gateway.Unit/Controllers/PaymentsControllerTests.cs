using AutoBogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PaymentGatewayAPI.Controllers;
using PaymentGatewayAPI.Exceptions;
using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Responses;
using PaymentGatewayAPI.Services.Interfaces;

namespace PaymentGatewayUnitAPI.Controllers;

internal class PaymentsControllerTests
{
    private PaymentsController _paymentsController;
    private ILogger<PaymentsController> _loggerMock;
    private ICreatePaymentService _createPaymentServiceMock;
    private IRetrievePaymentService _retrievePaymentServiceMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = Substitute.For<ILogger<PaymentsController>>();
        _createPaymentServiceMock = Substitute.For<ICreatePaymentService>();
        _retrievePaymentServiceMock = Substitute.For<IRetrievePaymentService>();

        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Scheme.Returns("scheme");
        httpContext.Request.Host.Returns(new HostString("host"));

        _paymentsController = new PaymentsController(_loggerMock);

        _paymentsController.ControllerContext.HttpContext = httpContext;
    }

    [Test]
    public async Task Post_ShouldReturnCreated_WhenPaymentIsSuccessfullyCreated()
    {
        // Arrange
        var createPaymentRequest = AutoFaker.Generate<CreatePaymentRequest>();
        var createdPaymentResponse = AutoFaker.Generate<PaymentResponse>();

        _createPaymentServiceMock
            .Execute(createPaymentRequest, Arg.Any<CancellationToken>() )
            .Returns(createdPaymentResponse);

        // Act
        var result = await _paymentsController.Post(createPaymentRequest, _createPaymentServiceMock);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdAtResult = (CreatedResult )result;
        createdAtResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdAtResult.Value.Should().BeEquivalentTo(createdPaymentResponse);
    }

    [Test]
    public async Task Post_ShouldReturnConflict_WhenPaymentAlreadyExists()
    {
        // Arrange
        var createPaymentRequest = AutoFaker.Generate<CreatePaymentRequest>();

        _createPaymentServiceMock
            .Execute(createPaymentRequest, Arg.Any<CancellationToken>())
            .Throws(new PaymentAlreadyExistsException());

        // Act
        var result = await _paymentsController.Post(createPaymentRequest, _createPaymentServiceMock);

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
        var paymentResponse = AutoFaker.Generate<PaymentResponse>();

        _retrievePaymentServiceMock
            .Execute(paymentId, Arg.Any<CancellationToken>())
            .Returns(paymentResponse);

        // Act
        var result = await _paymentsController.Get(paymentId, _retrievePaymentServiceMock);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(paymentResponse);
    }

    [Test]
    public async Task Get_ShouldReturnNotFound_WhenPaymentIsNotFound()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        _retrievePaymentServiceMock
            .Execute(paymentId, Arg.Any<CancellationToken>())
            .Returns((PaymentResponse) null!);

        // Act
        var result = await _paymentsController.Get(paymentId, _retrievePaymentServiceMock);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"Requested payment with id {paymentId} not found");
    }
}
