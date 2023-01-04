namespace Models
{
    public class ProductDTOFull
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ImageId { get; set; }
    }
}
