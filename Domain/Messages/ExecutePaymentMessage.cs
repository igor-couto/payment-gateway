using System.Text.Json.Serialization;

namespace Domain.Messages;

public class ExecutePaymentMessage
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("merchantId")]
    public Guid MerchantId { get; init; }

    [JsonPropertyName("amount")]
    public string Amount { get; init; } = null!;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; } = null!;

    [JsonPropertyName("CreditCardHolder")]
    public string CreditCardHolder { get; init; } = null!;

    [JsonPropertyName("CreditCardNumber")]
    public string CreditCardNumber { get; init; } = null!;

    [JsonPropertyName("CreditCardVerificationValue")]
    public string CreditCardVerificationValue { get; init; } = null!;

    [JsonPropertyName("CreditCardExpirityMonth")]
    public int CreditCardExpirityMonth { get; init; }

    [JsonPropertyName("CreditCardExpirityYear")]
    public int CreditCardExpirityYear { get; init; }
}
