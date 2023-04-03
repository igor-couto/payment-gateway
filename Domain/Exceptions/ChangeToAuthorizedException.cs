namespace Domain.Exceptions;

public class ChangeToAuthorizedException : Exception
{
    public ChangeToAuthorizedException(string id, string status) : base($"A payment {id} with the status of {status} cannot change to authorized") { }
}