using System.Text.Json.Serialization;

namespace Domain.Messages;

public class ExecutePaymentMessage
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("merchantId")]
    public Guid MerchantId { get; init; }

    [JsonPropertyName("amount")]
    public string Amount { get; init; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; init; }

    [JsonPropertyName("CreditCardHolder")]
    public string CreditCardHolder { get; init; }

    [JsonPropertyName("CreditCardNumber")]
    public string CreditCardNumber { get; init; }

    [JsonPropertyName("CreditCardVerificationValue")]
    public string CreditCardVerificationValue { get; init; }

    [JsonPropertyName("CreditCardExpirityMonth")]
    public int CreditCardExpirityMonth { get; init; }

    [JsonPropertyName("CreditCardExpirityYear")]
    public int CreditCardExpirityYear { get; init; }
}
