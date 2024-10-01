namespace MediaVerse.Client.Application.DTOs.Common;

public class Page<T>
{
    public List<T> Contents { get; set; } = new List<T>();
    public int? PageCount { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNext => CurrentPage < PageCount;

    public Page(List<T> contents, int currentPage, int itemCount, int size)
    {
        Contents = contents;
        CurrentPage = currentPage;
        PageCount = CalculatePageCount(itemCount, size);
        
    }

    public Page()
    {
    }

    private int CalculatePageCount(int itemCount, int size)
    {
        return  (itemCount + size - 1) / size;
    }
}