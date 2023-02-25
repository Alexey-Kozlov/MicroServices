using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace RabbitProducer.Services
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<UserAccessor> _logger;
        public UserAccessor(IHttpContextAccessor context, ILogger<UserAccessor> logger)
        {
            _context = context;
            _logger = logger;   
        }
        public string GetUserName()
        {        
            return _context.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.GivenName)!.Value;
        }
        public string GetUserId()
        {
            //return _context.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return _context.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
