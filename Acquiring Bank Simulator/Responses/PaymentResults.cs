namespace AcquiringBankSimulator.Responses;

internal static class PaymentResults
{
    internal static readonly Dictionary<int, Result> Results = new()
    {
        { 0, new Result("Authorized", "The transaction has been authorized successfully.")},
        { 1, new Result("Approved", "The transaction has gone through, and the payment is officially complete.")},
        { 2, new Result("Refer to issuer", "The transaction was rejected with no reason provided. The fix requires contacting the card-issuing bank for an explanation.")},
        { 3, new Result("Refer to issuer (special condition)", "The user’s bank rejected the transaction. You must contact the bank for the explanation and fix.")},
        { 4, new Result("Invalid merchant", "This code arises if the payment information is entered incorrectly or if your merchant account or credit card terminal isn’t configured properly. Try running the sale again — or contacting your payment processor directly.")},
        { 5, new Result("Pick up card", "The transaction was rejected, and you (the merchant) are instructed to hold onto the card until you have a chance to contact the customer’s bank.")},
        { 6, new Result("Do not honor", "The user’s bank instructs you not to accept payment — until you (the merchant) contact the bank directly.")},
        { 7, new Result("Error", "The card-issuing bank detected an error but can’t specify what it is. You can try the card again or call the bank directly for further instructions.")},
        { 8, new Result("Pick up card (special condition)", "The card-issuing bank has detected fraud. Both parties should contact the bank before using the card again.")},
        { 9, new Result("Partial Amount Approval", "The card-issuing bank only accepted a portion of the payment — normally due to insufficient funds or credit limits.")},
        { 10, new Result("Invalid Transaction", "The sale was rejected due to any number of issues (like when trying to reverse a charge). You can try the transaction again or call the card-issuing bank directly.")},
        { 11, new Result("Invalid Amount", "The transaction was declined — often because of invalid characters or symbols (e.g., # or &). Try inputting the amount again and see if the sale goes through.")},
        { 12, new Result("Invalid Card Number", "This is similar to the above, but instead of an incorrect amount, the card account number is wrong. Try initiating the sale again. If that fails, contact the bank directly.")},
        { 13, new Result("No Such Issuer", "The first digit of the card number was entered incorrectly. Try running the sale again by re-entering the account number more carefully.")},
        { 14, new Result("Customer Cancellation", "The customer has voided or reversed the transaction. The sale will not go through unless you use a different payment method.")},
        { 15, new Result("Re-enter Transaction", "The sale was declined due to an unknown error. Try running the sale again by re-entering all the information more carefully a second time.")},
        { 16, new Result("No reply/File Temporarily Unavailable", "The transaction was declined for an unspecified reason. Try running the sale again. If that doesn’t work, contact the card-issuing bank directly.")},
        { 17, new Result("Allowable PIN Tries Exceeded", "This code is also flagged as code 75 on some systems. The sale is rejected because the customer’s PIN has been entered incorrectly too many times. Ask for an alternate payment method.")},
        { 18, new Result("Lost Card, Pick up", "The card has been reported stolen or lost and cannot be used to complete the sale. Try the transaction again with a different payment method.")},
        { 19, new Result("Stolen Card, Pick up (fraud)", "The card has been reported stolen or lost and cannot be used for this transaction. Try running the sale again with an alternate payment method.")},
        { 20, new Result("Insufficient Funds", "The customer’s account doesn’t have enough funds to cover the sale. Ask for an alternate payment method or contact the user’s bank directly.")},
        { 21, new Result("Expired Card", "The customer’s card has expired and is no longer valid. Ask for an alternate payment method.")},
        { 22, new Result("Transaction Not Permitted (Cardholder)", "The card is valid — but not for that particular sale or transaction. Ask for an alternate payment method.")},
        { 23, new Result("Transaction Not Permitted (terminal/merchant)", "Your merchant account or credit card terminal isn’t configured properly. You may need to switch to cash or check to complete the sale. You should also follow up with your payment processor to fix your merchant account setup.")},
        { 24, new Result("Suspected Fraud", "the card-issuing bank has detected fraud. Ask for an alternate payment method (and contact the bank directly).")},
        { 25, new Result("Restricted Card, invalid service", "This code happens in cases where your merchant account doesn’t support the customer’s card network (e.g., Discover or American Express). Invalid service codes also apply to eCommerce transactions among cards that cannot be used to shop online.")},
        { 26, new Result("Security Violation", "The CVV, CVC or CID security code on the back of the card wasn’t entered correctly. Try running the entire transaction again with the proper security code, expiration date and account number.")},
        { 27, new Result("Activity Limit Exceeded", "The sale would push the customer over her or his credit limit. Ask the customer for an alternative payment method.")},
        { 28, new Result("No reason/system unavailable", "This happens whenever communication errors exist between the card-issuing bank and merchant’s payment environment. Try running the transaction again after a few minutes.")},
        { 29, new Result("Issuer or Switch is not available", "Either the payment processor or terminal was unable to complete the transaction. Try running the sale again. If that fails, contact the bank and payment processor directly.")},
        { 30, new Result("Institution not found/unable to route transaction", "The credit card terminal cannot reach the card-issuing bank. Try running the sale again after a few minutes — or contact the card-issuing bank directly.")},
        { 31, new Result("Legal Violation", "The card-issuing bank has frozen the customer’s account (for any number of reasons). Call the bank directly for next steps.")},
        { 32, new Result("System Error/Malfunction", "The payment processor has experienced an error. Try running the sale again — or contact the payment processor directly.")}
    };
}