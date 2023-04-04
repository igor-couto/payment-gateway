namespace Domain.Exceptions;

public class PaymentChangeToFinishedException : Exception
{
    public PaymentChangeToFinishedException(string id, string status) : base($"A payment {id} with the status of {status} cannot change to finished") { }
}