namespace AppCore.Models;

public class FindResult<T>
{
    public List<T> Items { get; set; }

    public int TotalCount { get; private set; }

    public static FindResult<T> Success(List<T> items, long totalCount)
    {
        return new FindResult<T> { Items = items, TotalCount = (int)totalCount };
    }

    public static FindResult<T> Empty()
    {
        return new FindResult<T> { Items = [], TotalCount = 0 };
    }
}