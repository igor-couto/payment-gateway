namespace PaymentExecutor.Exceptions;

public class UnknownPaymentPaymentMessageException : Exception
{
    private new const string Message = "Queue payment message does not match any database payment.";

    public UnknownPaymentPaymentMessageException() : base(Message) { }
}