using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Identity.Services
{
    public class RolesService: IRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<List<RoleDTO>>> GetRolesList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Result<List<RoleDTO>>.Success(roles.Select(p => new RoleDTO { Id = p.Id, Name = p.Name }).ToList());
        }

        public async Task<Result<object>> DeleteUserFromRole(string roleId, string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == login);
            if (user == null) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка удаления пользователя из роли - пользователь c логином '" + login + "' не найден");
            var role = await _roleManager.Roles.FirstOrDefaultAsync(p => p.Id == roleId);
            if (role == null) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка удаления пользователя из роли - роль с ID = " + roleId + " не найден");
            if (!_userManager.IsInRoleAsync(user, role.Name).Result) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка удаления пользователя из роли - пользователь " + user.UserName +
                " не найден в роли " + role.Name);
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return Result<object>.Success(true);
        }

        public async Task<Result<object>> AddUserToRole(string roleId, string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == login);
            if (user == null) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка добавления пользователя в роль - пользователь c логином '" + login + "' не найден");
            var role = await _roleManager.Roles.FirstOrDefaultAsync(p => p.Id == roleId);
            if (role == null) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка добавления пользователя в роль - роль с ID = " + roleId + " не найдена");
            if (_userManager.IsInRoleAsync(user, role.Name).Result) return Result<object>.Error(ErrorEnum.NotFoundError,
                "Ошибка добавления пользователя в роль - пользователь " + user.UserName +
                " уже в роли " + role.Name);
            await _userManager.AddToRoleAsync(user, role.Name);
            return Result<object>.Success(true);
        }
    }
}
