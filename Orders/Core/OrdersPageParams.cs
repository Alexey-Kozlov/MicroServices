﻿namespace OrdersAPI.Core
{
    public class OrdersPageParams : PagingParams<OrdersPageSettings>
    {
        public string UserId { get; set; }
    }
}