using System.Text;
using FluentValidation;

namespace PaymentGatewayAPI.Requests.Validators;

public class CreditCardRequestValidator : AbstractValidator<CreditCardRequest>
{
    public CreditCardRequestValidator()
    {
        RuleFor(request => request.Holder)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.CardNumber)
            .NotEmpty()
            .Must(BeValidCreditCardNumber)
                .WithMessage("The card number is invalid. A card number must be the correct size and valid");

        RuleFor(request => request.CardVerificationValue)
            .NotEmpty()
            .Length(min: 3, max: 4)
            .Must(BeOnlyDigits)
                .WithMessage("The Card Verification Value provided is not valid. The value must only contain numbers.");

        RuleFor(request => request.ExpirityYear)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Year);

        RuleFor(request => request.ExpirityMonth)
            .NotEmpty()
            .InclusiveBetween(1, 12)
            .When(request => IsGreaterOrEqualThanToday(request.ExpirityYear!.Value, request.ExpirityMonth!.Value))
                .WithMessage("Card is expired. The expiration date must be greater than the current date");
    }

    private static bool BeOnlyDigits(string cardVerificationValue)
    {
        return cardVerificationValue.All(char.IsDigit);
    }

    private static bool IsGreaterOrEqualThanToday(int year, int month) 
    {
        var expirityDate = new DateTime(year, month, 1).ToUniversalTime();
        var today = DateTime.Today.ToUniversalTime();

        return expirityDate >= today;
    }

    private static bool BeValidCreditCardNumber(string? cardNumber) 
    {
        var onlyDigitsCardNumber = RemoveNonNumeric(cardNumber!);
        if (onlyDigitsCardNumber.Length > 19 || onlyDigitsCardNumber.Length < 12)
            return false;

        return IsValidWithLuhnAlgorithm(onlyDigitsCardNumber);
    }

    private static bool IsValidWithLuhnAlgorithm(string onlyDigitsCardNumber) 
    {
        var sum = 0;
        for (var index = 0; index < onlyDigitsCardNumber.Length; index++)
        {
            int digit = onlyDigitsCardNumber[index];

            if (index % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        return sum % 10 == 0;
    }

    private static string RemoveNonNumeric(string input)
    {
        var stringBuilder = new StringBuilder();
        foreach (var character in input)
        {
            if (char.IsDigit(character))
                stringBuilder.Append(character);
        }
        return stringBuilder.ToString();
    }
}
