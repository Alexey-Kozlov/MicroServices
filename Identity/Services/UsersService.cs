using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.DbContexts;
using Identity.Helpers;
using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services
{
    public class UsersService: IUsersService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsersService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<UserDTO>>> GetUsersByRoleId(UserRoleParams pagingParams)
        {
            var query = GetUsersQuery(pagingParams.RoleId);

            if (!string.IsNullOrEmpty(pagingParams.UserSearch))
            {
                query = query.Where(p => p.DisplayName.ToLower().Contains(pagingParams.UserSearch.ToLower()));
            }
            //обязательно сортировать для педжинации по страницам, иначе некорректное отображение
            query = query.OrderBy(p => p.Login);
            return Result<List<UserDTO>>.Success(await query.ToListAsync());
        }


        private IQueryable<UserDTO> GetUsersQuery(string roleId)
        {
            var query = Enumerable.Empty<UserDTO>().AsQueryable();
            if (string.IsNullOrEmpty(roleId))
            {
                query = _context.Users.AsNoTracking()
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .Distinct();
            }
            else
            {
                query = _context.UserRoles.AsNoTracking()
                .Where(p => p.RoleId == roleId)
                .Join(_context.Users.AsNoTracking(), r => r.UserId, u => u.Id, (r, u) => u)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
            }
            return query;
        }
    }
}
