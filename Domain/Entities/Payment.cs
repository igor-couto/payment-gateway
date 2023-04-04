using System.Text;
using Domain.Exceptions;

namespace Domain.Entities;

public record Payment
{
    public Guid Id { get; init; }
    public Guid CheckoutId { get; init; }
    public Guid ShopperId { get; init; }
    public Guid MerchantId { get; init; }
    public string Amount { get; init; }
    public string Currency { get; init; }
    public string MaskedCreditCardNumber { get; init; }
    public PaymentStatus PaymentStatus { get; private set; }
    public string? StatusMessage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Payment(){ }

    public Payment(
        Guid checkoutId,
        Guid shopperId,
        Guid merchantId,
        string amount,
        string currency,
        string creditCardNumber)
    {
        Id = Guid.NewGuid();
        CheckoutId = checkoutId;
        ShopperId = shopperId;
        MerchantId = merchantId;
        Amount = amount;
        Currency = currency;
        MaskedCreditCardNumber = CreateMaskedCreditCardNumber(creditCardNumber);
        CreatedAt = DateTime.UtcNow;
        PaymentStatus = PaymentStatus.NotStarted;
        StatusMessage = default;
        UpdatedAt = default;
    }

    private static string CreateMaskedCreditCardNumber(string creditCardNumber)
    {
        var stringBuilder = new StringBuilder(creditCardNumber);

        var startIndex = stringBuilder.Length - 4;
        for (var index = 0; index < stringBuilder.Length; index++)
        {
            if (index < startIndex)
            {
                if (char.IsDigit(stringBuilder[index]))
                    stringBuilder[index] = '*';
                else
                    stringBuilder[index] = ' ';
            }
            else if (!char.IsDigit(stringBuilder[index]))
            {
                stringBuilder[index] = ' ';
            }
        }

        return stringBuilder.ToString();
    }

    public void Authorize()
    {
        if (PaymentStatus is PaymentStatus.NotStarted)
        {
            UpdatedAt = DateTime.UtcNow;
            PaymentStatus = PaymentStatus.Authorized;
        }
        else throw new PaymentChangeToAuthorizedException(Id.ToString(), PaymentStatus.ToString());
    }

    public void Finish()
    {
        if (PaymentStatus is PaymentStatus.Authorized)
        {
            UpdatedAt = DateTime.UtcNow;
            PaymentStatus = PaymentStatus.Success;
        }
        else throw new PaymentChangeToFinishedException(Id.ToString(), PaymentStatus.ToString());
    }

    public void Fail(string failReason)
    {
        UpdatedAt = DateTime.UtcNow;
        PaymentStatus = PaymentStatus.Failed;
        StatusMessage = failReason;
    }
}