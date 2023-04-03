namespace PaymentGatewayAPI.Exceptions;

public class PaymentAlreadyExistsException : Exception
{
    private new const string Message = "Could not create payment because it already exists.";

    public PaymentAlreadyExistsException() : base(Message) {}
}
