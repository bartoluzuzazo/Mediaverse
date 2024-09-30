namespace MediaVerse.Domain.Exceptions;

public class NotAuthorizedException : Exception
{
    public NotAuthorizedException(string? message) : base(message)
    {
    }

    public NotAuthorizedException()
    {
    }
}