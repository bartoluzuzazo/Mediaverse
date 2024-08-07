namespace MediaVerse.Domain.AggregatesModel;

public class BaseResponse<T>
{
    public BaseResponse(T data)
    {
        Data = data;
    }

    public BaseResponse(Exception exception)
    {
        Exception = exception;
    }

    public T? Data { get; set; }
    public Exception? Exception { get; set; }
}