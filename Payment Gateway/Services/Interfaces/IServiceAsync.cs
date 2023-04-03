namespace PaymentGatewayAPI.Services.Interfaces;

public interface IServiceAsync<in TIn, TOut>
{
    Task<TOut> Execute(TIn request, CancellationToken cancellationToken = default);
}
