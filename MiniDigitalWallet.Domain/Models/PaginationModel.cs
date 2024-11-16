using Microsoft.EntityFrameworkCore;

namespace MiniDigitalWallet.Domain.Features;

public class PaginationModel<T> : List<T>
{
    public int PageNo { get; private set; }
    public int TotalPages { get; private set; }

    public PaginationModel(List<T> items, int count, int pageNo, int pageSize)
    {
        PageNo = pageNo;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public bool HasPreviousPage => PageNo > 1;
    public bool HasNextPage => PageNo < TotalPages;

    public static async Task<PaginationModel<T>> CreateAsync(IQueryable<T> source, int pageNo, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginationModel<T>(items, count, pageNo, pageSize);
    }
}
