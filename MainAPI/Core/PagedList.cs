using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MainAPI.Core
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

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
