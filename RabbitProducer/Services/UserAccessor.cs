using System.Security.Claims;

namespace RabbitProducer.Services
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _context;
        public UserAccessor(IHttpContextAccessor context)
        {
            _context = context;
        }
        public string GetUserName()
        {
            return _context.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
        }
        public string GetUserId()
        {
            return _context.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }
    }
}
