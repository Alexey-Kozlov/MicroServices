namespace OrdersAPI.Domain
{
    public class ProductRef
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
    }
}
