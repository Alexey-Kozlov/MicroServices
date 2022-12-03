namespace ImageAPI.Domain
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public byte[] Data { get; set; }
    }
}
