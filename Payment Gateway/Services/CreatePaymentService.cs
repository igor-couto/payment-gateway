using Infrastructure.Persistence;
using Domain.Entities;
using Domain.Messages;
using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Services.Interfaces;
using PaymentGatewayAPI.Exceptions;
using PaymentGatewayAPI.Responses;

namespace PaymentGatewayAPI.Services;

public class CreatePaymentService : ICreatePaymentService
{
    private readonly ILogger<CreatePaymentService> _logger;
    private readonly IExecutePaymentMessagePublisherService _executePaymentPublisher;
    private readonly DatabaseContext _databaseContext;

    public CreatePaymentService(ILogger<CreatePaymentService> logger, DatabaseContext databaseContext, IExecutePaymentMessagePublisherService executePaymentMessagePublisherService)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        _executePaymentPublisher = executePaymentMessagePublisherService;
    }

    public async Task<PaymentResponse> Execute(CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken)
    {
        var payment = CreatePaymentFromRequest(createPaymentRequest);

        if (PaymentAlreadyExists(createPaymentRequest.CheckoutId))
            throw new PaymentAlreadyExistsException();
        
        using var transaction = _databaseContext.Database.BeginTransaction();
        try
        {
            await SavePayment(payment, cancellationToken);
            await SendExecutePaymentMessage(payment.Id, createPaymentRequest, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Failed to register payment: {exception.Message}");
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }

        _logger.LogInformation($"Payment {createPaymentRequest.CheckoutId} registered successfully");

        return new PaymentResponse(payment);
    }

    private bool PaymentAlreadyExists(Guid checkoutId)
    {
        var recordedPayment = _databaseContext.Payments.FirstOrDefault(payment => payment.CheckoutId == checkoutId);

        if (recordedPayment is not null)
        {
            _logger.LogInformation("Requested payment already exists with checkout {checkoutId}", checkoutId);
            return true;
        }
        return false;
    }

    private Payment CreatePaymentFromRequest(CreatePaymentRequest createPaymentRequest) 
    {
        try
        {
            var payment = new Payment(
                checkoutId: createPaymentRequest.CheckoutId,
                shopperId: createPaymentRequest.ShopperId,
                merchantId: createPaymentRequest.MerchantId,
                amount: createPaymentRequest.Amount!,
                currency: createPaymentRequest.Currency!,
                creditCardNumber: createPaymentRequest.CreditCard.CardNumber!
            );

            return payment;
        }
        catch (Exception exception)
        {
            _logger.LogError("An error occurred when trying to create a payment: {Message}", exception.Message);
            throw;
        }
    }

    private async Task SavePayment(Payment payment, CancellationToken cancellationToken) 
    {
        await _databaseContext.Payments.AddAsync(payment, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SendExecutePaymentMessage(Guid id, CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken)
    {
        await _executePaymentPublisher.Publish(new ExecutePaymentMessage
        {
            Id = id,
            MerchantId = createPaymentRequest.MerchantId,
            CurrencyCode = createPaymentRequest.Currency!,
            Amount = createPaymentRequest.Amount!,
            CreditCardHolder = createPaymentRequest.CreditCard.Holder!,
            CreditCardNumber = createPaymentRequest.CreditCard.CardNumber!,
            CreditCardVerificationValue= createPaymentRequest.CreditCard.CardVerificationValue!,
            CreditCardExpirityMonth = createPaymentRequest.CreditCard.ExpirityMonth!.Value,
            CreditCardExpirityYear = createPaymentRequest.CreditCard.ExpirityYear!.Value,
        }, cancellationToken);
    }
}
