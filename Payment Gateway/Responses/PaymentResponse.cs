using System.Text.Json.Serialization;
using Domain.Entities;

namespace PaymentGatewayAPI.Responses;

public class PaymentResponse
{

    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("checkoutId")]
    public Guid CheckoutId { get; init; }
    
    [JsonPropertyName("shopperId")]
    public Guid ShopperId { get; init; }
    
    [JsonPropertyName("merchantId")]
    public Guid MerchantId { get; init; }
    
    [JsonPropertyName("amount")]
    public string Amount { get; init; }

    [JsonPropertyName("currency")]
    public string Currency { get; init; }
    
    [JsonPropertyName("maskedCreditCardNumber")]
    public string MaskedCreditCardNumber { get; init; }
    
    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; init; }

    [JsonPropertyName("statusMessage")]
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
