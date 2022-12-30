namespace OrdersAPI.Domain
{
    public class ProductRef
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public Order Order { get; set; }
    }
}
