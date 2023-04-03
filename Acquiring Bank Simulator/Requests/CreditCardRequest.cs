using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Requests;

public readonly record struct CreditCardRequest
{
    [JsonPropertyName("holder")]
    public string Holder { get; init; }

    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; init; }

    [JsonPropertyName("cardVerificationValue ")]
    public string CardVerificationValue { get; init; }

    [JsonPropertyName("expirityMonth")]
    public int ExpirityMonth { get; init; }

    [JsonPropertyName("expirityYear")]
    public int ExpirityYear { get; init; }
}