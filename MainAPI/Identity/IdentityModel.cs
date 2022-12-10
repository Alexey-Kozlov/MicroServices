namespace MIdentity
{
    public class IdentityModel
    {
        public string token { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
