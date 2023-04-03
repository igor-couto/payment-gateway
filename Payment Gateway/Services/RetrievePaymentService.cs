using Infrastructure.Persistence;
using PaymentGatewayAPI.Responses;
using PaymentGatewayAPI.Services.Interfaces;

namespace PaymentGatewayAPI.Services;

public class RetrievePaymentService : IRetrievePaymentService
{
    private readonly ILogger<RetrievePaymentService> _logger;
    private readonly DatabaseContext _databaseContext;

    public RetrievePaymentService(ILogger<RetrievePaymentService> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task<PaymentResponse?> Execute(Guid payementId, CancellationToken cancellationToken = default)
    {
        var payment = await _databaseContext.Payments.FindAsync(payementId);
        
        if(payment is null)
            return null;

        return new PaymentResponse(payment);
    }
}