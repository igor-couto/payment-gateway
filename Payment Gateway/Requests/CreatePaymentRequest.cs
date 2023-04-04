using System.Text.Json.Serialization;

namespace PaymentGatewayAPI.Requests;

public record CreatePaymentRequest
{
    /// <summary>
    /// This is the id that represents the checkout process in the purchase made by the shopper with the merchant.
    /// It is used as an idempotency key to ensure that this payment will be unique
    /// </summary>
    /// <example>f4d2e36b-7816-4e4b-9ec9-a038598e9a1c</example>
    [JsonPropertyName("checkoutId")]
    public Guid CheckoutId { get; init; }

    /// <summary>
    /// This is the id that represents the shopper (buyer) making the payment.
    /// </summary>
    /// <example>7d77780b-2ee2-40c4-bf6e-b4b8240a23af</example>
    [JsonPropertyName("shopperId")]
    public Guid ShopperId { get; init; }

    /// <summary>
    /// This is the id that represents the merchant receiving the payment.
    /// </summary>
    /// <example>d3b4c6e8-c5a6-475d-87ad-37f634d8e16e</example>
    [JsonPropertyName("merchantId")]
    public Guid MerchantId { get; init; }

    /// <summary>
    /// Represents the payment value that should be charged for the transaction, expressed in the format "00.00".
    /// It should be noted that this field should only contain numerical values and the decimal point, with no other characters such as commas or currency symbols.
    /// </summary>
    /// <example>23.50</example>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>
    /// Currency used in this payment. It is represented in three letters alphabetic code using the ISO 4217 format.
    /// </summary>
    /// <example>USD</example>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>
    /// Information about the credit card used for payment.
    /// </summary>
    [JsonPropertyName("creditCard")]
    public CreditCardRequest CreditCard { get; init; }
}