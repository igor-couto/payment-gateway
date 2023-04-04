namespace Domain.Exceptions;

public class PaymentChangeToAuthorizedException : Exception
{
    public PaymentChangeToAuthorizedException(string id, string status) : base($"A payment {id} with the status of {status} cannot change to authorized") { }
}