namespace MediaVerse.Domain.Exceptions;

public class ProblemException : Exception
{
    public ProblemException()
    {
    }

    public ProblemException(string? message) : base(message)
    {
    }
}