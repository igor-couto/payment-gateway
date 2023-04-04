namespace AcquiringBankSimulator.Responses;

internal static class PaymentResults
{
    internal static readonly Dictionary<int, Result> Results = new()
    {
        { 1, new Result("Approved", "The transaction has gone through, and the payment is officially complete.")},
        { 2, new Result("Refer to issuer", "The user’s bank rejected the transaction.")},
        { 3, new Result("Lost or stolen card", "The card has been reported stolen or lost and cannot be used to complete the sale.")},
        { 4, new Result("Insufficient Funds", "The customer’s account doesn’t have enough funds to cover the sale.")},
        { 5, new Result("Suspected Fraud", "The card-issuing bank has detected fraud.")},
        { 6, new Result("Security code could not be validated", "The CVV, CVC or CID security code on the back of the card wasn’t entered correctly")},
        { 7, new Result("Activity Limit Exceeded", "The sale would push the customer over her or his credit limit.")},
        { 8, new Result("Invalid Merchant", "The payment information is entered incorrectly or the merchant account or credit card terminal isn’t configured properly.")},
        { 9, new Result("Legal Violation", "The card-issuing bank has frozen the customer’s account (for any number of reasons).")},
        { 10, new Result("System Error/Malfunction", "The payment processor has experienced an error. Try running the sale again — or contact the payment processor directly.")}
    };
}