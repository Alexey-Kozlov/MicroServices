using Identity.Helpers;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Identity.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UsersHelper _userHelper;
        protected ResponseDTO _response;

        public IdentityController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, RoleManager<IdentityRole> roleManager, 
            UsersHelper userHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userHelper = userHelper;
            this._response = new ResponseDTO();
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<ResponseDTO>> GetCurrentUser()
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(p => p.UserName == User.FindFirstValue(ClaimTypes.Name));
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { "Ошибка логина или пароля" };
                return Unauthorized(_response);
            }
            _response.Result = await _userHelper.CreateUserDTO(user);
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(p => p.UserName == loginDTO.Login);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { "Ошибка логина или пароля" };
                return Unauthorized(_response);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (result.Succeeded)
            {
                _response.Result = await _userHelper.CreateUserDTO(user);
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Errors = new List<string>() { "Ошибка логина или пароля" };
            return Unauthorized(_response);
        }

        [HttpPost]
        [Route("CheckToken")]
        public async Task<IPrincipal> CheckToken([FromBody]IdentityModel _data)
        {

            var principal = await Task.FromResult(_tokenService.ValidateToken(_data.token));
            return principal;
        }
    }
}
