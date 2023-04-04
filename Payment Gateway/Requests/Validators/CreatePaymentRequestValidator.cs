using FluentValidation;

namespace PaymentGatewayAPI.Requests.Validators;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public static readonly string[] CurrencyCodes = { "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BTC", "BTN", "BWP", "BYN", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNH", "CNY", "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MRU", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "SSP", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VES", "VND", "VUV", "WST", "XAF", "XCD", "XDR", "XOF", "XPF", "YER", "ZAR", "ZMW", "ZWL" };

    public CreatePaymentRequestValidator()
    {
        RuleFor(createPaymentRequest => createPaymentRequest.CheckoutId)
            .NotEmpty();

        RuleFor(createPaymentRequest => createPaymentRequest.MerchantId)
            .NotEmpty();

        RuleFor(createPaymentRequest => createPaymentRequest.ShopperId)
            .NotEmpty();

        RuleFor(createPaymentRequest => createPaymentRequest.Currency)
            .NotEmpty()
            .Length(3)
            .Must(BeCorrectCurrencyAlphabeticCode).WithMessage("The given currency is not known. Please provide a three-letter alphabetic code belonging to the ISO 4217 standard");

        RuleFor(createPaymentRequest => createPaymentRequest.Amount)
            .NotEmpty()
            .Must(BeInMonetaryFormat).WithMessage("The amount provided is not in currency format. The value must be dot-separated and contain numbers only.");

        RuleFor(createPaymentRequest => createPaymentRequest.CreditCard)
            .NotEmpty();
    }

    private static bool BeCorrectCurrencyAlphabeticCode(string currencyCode) 
        => CurrencyCodes.Contains(currencyCode);
    
    private static bool BeInMonetaryFormat(string? monetaryValue) 
    {
        if (monetaryValue is null)
            return false;

        var currentIndex = 0;

        while (currentIndex < monetaryValue.Length && monetaryValue[currentIndex] is not '.')
        {
            if (!char.IsDigit(monetaryValue[currentIndex]))
                return false;
            
            currentIndex++;
        }

        if (currentIndex == monetaryValue.Length || monetaryValue.Length - (currentIndex + 1) > 2 || currentIndex == monetaryValue.Length - 1)
            return false;

        currentIndex++;
        while (currentIndex < monetaryValue.Length)
        {
            if (!char.IsDigit(monetaryValue[currentIndex]))
                return false;
            
            currentIndex++;
        }

        return true;
    }
}
