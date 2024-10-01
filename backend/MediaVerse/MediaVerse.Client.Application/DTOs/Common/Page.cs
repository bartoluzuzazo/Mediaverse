namespace MediaVerse.Client.Application.DTOs.Common;

public class Page<T>
{
    public List<T> Contents { get; set; } = new List<T>();
    public int PageCount { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNext => CurrentPage < PageCount;

}