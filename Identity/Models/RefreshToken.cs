namespace Identity.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.Now.AddDays(7);
        public bool IsExpired => DateTime.Now > Expires;
        public DateTime? Revoked { get; set; }
        public bool IaActive => Revoked == null && !IsExpired;
    }
}
