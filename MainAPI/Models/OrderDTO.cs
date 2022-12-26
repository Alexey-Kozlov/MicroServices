namespace Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductItemsDTO> Products { get; set; }
    }
}
