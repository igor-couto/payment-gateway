namespace Domain.Exceptions;

public class ChangeToFinishedException : Exception
{
    public ChangeToFinishedException(string id, string status) : base($"A payment {id} with the status of {status} cannot change to finished") { }
}