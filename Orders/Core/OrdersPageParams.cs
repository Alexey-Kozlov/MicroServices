namespace OrdersAPI.Core
{
    public class OrdersPageParams : PagingParams<OrdersPageSettings>
    {
        public string UserId { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}
