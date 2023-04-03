using Domain.Messages;

namespace PaymentGatewayAPI.Services.Interfaces;

public interface IExecutePaymentMessagePublisherService
{
    Task Publish(ExecutePaymentMessage executePaymentMessage, CancellationToken cancellationToken = default);
}
