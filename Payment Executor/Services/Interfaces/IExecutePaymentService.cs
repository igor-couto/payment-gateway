using Domain.Messages;

namespace PaymentExecutor.Services.Interfaces;

public interface IExecutePaymentService {
    Task Execute(ExecutePaymentMessage executePaymentMessage, CancellationToken cancellationToken = default);
}
