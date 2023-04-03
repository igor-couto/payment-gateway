namespace PaymentExecutor.Exceptions;

public class FailedToCommunicateWithAcquiringBankException : Exception
{
    public FailedToCommunicateWithAcquiringBankException(string uri) : base($"Communication failure with the acquiring bank. Trying to request uri: {uri}") { }
}
