using Domain.Messages;
using System.Text.Json.Serialization;

namespace PaymentExecutor.Requests;

public readonly record struct CreateAuthorizationRequest
{
    [JsonPropertyName("merchantId ")]
    public Guid MerchantId { get; init; }

    [JsonPropertyName("amount")]
    public string Amount { get; init; }

    [JsonPropertyName("currency")]
    public string Currency { get; init; }

    [JsonPropertyName("creditCard")]
    public CreditCardRequest CreditCard { get; init; }

    public CreateAuthorizationRequest(ExecutePaymentMessage executePaymentMessage) 
    {
        MerchantId = executePaymentMessage.MerchantId;
        Amount = executePaymentMessage.Amount;
        Currency = executePaymentMessage.CurrencyCode;
        CreditCard = new CreditCardRequest
        {
            Holder = executePaymentMessage.CreditCardHolder,
            CardNumber = executePaymentMessage.CreditCardNumber,
            CardVerificationValue = executePaymentMessage.CreditCardVerificationValue,
            ExpirityMonth = executePaymentMessage.CreditCardExpirityMonth,
            ExpirityYear = executePaymentMessage.CreditCardExpirityYear
        };
    }
}