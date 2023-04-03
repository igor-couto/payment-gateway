namespace PaymentExecutor.Exceptions;

public class FailedToExecutePaymentException : Exception
{
    public FailedToExecutePaymentException(string id, string reason) : base($"Payment with id {id} failed. Reason : {reason}") { }
}