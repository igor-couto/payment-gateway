using System.Text.Json.Serialization;

namespace PaymentGatewayAPI.Requests;

public readonly record struct CreditCardRequest
{
    /// <summary>
    /// The name of the credit card owner, usually printed on the front of the card.
    /// </summary>
    /// <example>Igor Freitas Couto</example>
    [JsonPropertyName("holder")]
    public string? Holder { get; init; }

    /// <summary>
    /// A credit card number is the long set of digits that usually appear on the front or back of your credit card.
    /// It is used to identify your credit card account.
    /// </summary>
    /// <example>4980-5730-7604-7952</example>
    [JsonPropertyName("cardNumber")]
    public string? CardNumber { get; init; }

    /// <summary>
    /// The Card Verification Value (CVV), also known as Card Security Code (CSC) is a number that is usually printed on the back of a credit card. 
    /// The code is a security feature that allows the credit card processor to identify the cardholder.
    /// </summary>
    /// <example>123</example>
    [JsonPropertyName("cardVerificationValue ")]
    public string? CardVerificationValue { get; init; }

    /// <summary>
    /// Credit card expiration month.
    /// </summary>
    /// <example>3</example>
    [JsonPropertyName("ExpirityMonth")]
    public int? ExpirityMonth { get; init; }

    /// <summary>
    /// Credit card expiration year.
    /// </summary>
    /// <example>2024</example>
    [JsonPropertyName("ExpirityYear")]
    public int? ExpirityYear { get; init; }
}