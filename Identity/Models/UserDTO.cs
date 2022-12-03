namespace Identity.Models
{
    public class UserDTO
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Login { get; set; }
        public bool IsAdmin { get; set; }
    }
}
