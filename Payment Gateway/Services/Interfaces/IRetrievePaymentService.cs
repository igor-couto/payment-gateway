using Domain.Entities;
using PaymentGatewayAPI.Responses;

namespace PaymentGatewayAPI.Services.Interfaces;

public interface IRetrievePaymentService : IServiceAsync<Guid, PaymentResponse?> { }