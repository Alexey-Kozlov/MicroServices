using Microsoft.Extensions.Options;

namespace OrdersAPI.Core
{
    public class PagingParams<T> where T : PageSettings
    {
        private int _pageSize = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    return GetService().Value.PageSize;
                }
                else
                {
                    return _pageSize;
                }
            }
            set => _pageSize = value;
        }

        private static IOptions<T> GetService()
        {
            //вызываем нужный сервис в зависимости от T
            return (IOptions<T>)CoreServiceProvider.Provider.GetService(typeof(IOptions<T>))!;
        }
    }
}
