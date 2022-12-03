using Identity.Helpers;
using Identity.Models;

namespace Identity.Services
{
    public interface IUsersService
    {
        Task<Result<List<UserDTO>>> GetUsersByRoleId(UserRoleParams pagingParams);
    }
}
