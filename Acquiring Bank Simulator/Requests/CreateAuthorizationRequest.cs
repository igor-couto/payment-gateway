using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Requests;

public readonly record struct CreateAuthorizationRequest
{
    [JsonPropertyName("merchantId")]
    public Guid MerchantId { get; init; }

    [JsonPropertyName("amount")]
    public string Amount { get; init; }

    [JsonPropertyName("currency")]
    public string Currency { get; init; }

    [JsonPropertyName("creditCard")]
    public CreditCardRequest CreditCard { get; init; }
}