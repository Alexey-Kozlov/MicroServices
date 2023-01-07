using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrdersAPI.Core
{
    [JsonObject]
    public class PagedList<T>
    {
        [JsonProperty]
        public int CurrentPage { get; set; }
        [JsonProperty]
        public int TotalPages { get; set; }
        [JsonProperty]
        public int PageSize { get; set; }
        [JsonProperty]
        public int TotalCount { get; set; }
        [JsonProperty]
        public IEnumerable<T> OrderDTO { get; set; } = Enumerable.Empty<T>();

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            OrderDTO = items;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, OrdersPageParams pagingParams)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();
            return new PagedList<T>(items, count, pagingParams.PageNumber, pagingParams.PageSize);
        }
    }
}
