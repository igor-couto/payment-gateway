using Domain.Entities;

namespace PaymentGatewayAPI.Responses;

public class PaymentResponse
{
    public Guid Id { get; init; }
    public Guid CheckoutId { get; init; }
    public Guid ShopperId { get; init; }
    public Guid MerchantId { get; init; }
    public string Amount { get; init; }
    public string Currency { get; init; }
    public string MaskedCreditCardNumber { get; init; }
    public string PaymentStatus { get; init; }
    public string? StatusMessage { get; init; }

    public PaymentResponse(Payment payment)
    {
        Id = payment.Id;
        CheckoutId = payment.CheckoutId;
        ShopperId = payment.ShopperId;
        MerchantId = payment.MerchantId;
        Amount = payment.Amount;
        Currency = payment.Currency;
        MaskedCreditCardNumber = payment.MaskedCreditCardNumber;
        PaymentStatus = payment.PaymentStatus.ToString();
        StatusMessage = payment.StatusMessage;
    }
}
