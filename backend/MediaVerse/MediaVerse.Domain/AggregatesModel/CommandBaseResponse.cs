namespace MediaVerse.Domain.AggregatesModel;

public class CommandBaseResponse<T>
{
    public CommandBaseResponse(T? data)
    {
        Data = data;
    }

    public CommandBaseResponse(Exception? exception)
    {
        Exception = exception;
    }

    public T? Data { get; set; }
    public Exception? Exception { get; set; }
}