using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Responses;

namespace PaymentGatewayAPI.Services.Interfaces;

public interface ICreatePaymentService : IServiceAsync<CreatePaymentRequest, PaymentResponse> { }
