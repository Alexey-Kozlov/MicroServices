using Identity.Helpers;
using Identity.Models;

namespace Identity.Services
{
    public interface IRolesService
    {
        Task<Result<List<RoleDTO>>> GetRolesList();
        Task<Result<object>> DeleteUserFromRole(string roleId, string login);
        Task<Result<object>> AddUserToRole(string roleId, string login);
    }
}
