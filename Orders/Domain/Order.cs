namespace OrdersAPI.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public ICollection<ProductRef> ProductIdList { get; set; } = new List<ProductRef>();
    }
}
