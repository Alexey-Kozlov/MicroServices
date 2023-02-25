using Identity.Helpers;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Route("ms/identity/api")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UsersHelper _userHelper;
        private readonly IBrokerService _brokerService;
        private readonly ILogger<IdentityController> _logger;
        protected ResponseDTO _response;

        public IdentityController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, RoleManager<IdentityRole> roleManager, 
            UsersHelper userHelper, IBrokerService brokerService,
            ILogger<IdentityController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userHelper = userHelper;
            _brokerService= brokerService;
            this._response = new ResponseDTO();
            _logger= logger;
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<ResponseDTO>> GetCurrentUser()
        {
            var identityModel = new IdentityModel();
            identityModel.token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if(!await CheckToken(identityModel))
            {
                return Unauthorized(_response);
            }
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
            _logger.LogInformation("Login - " + JsonConvert.SerializeObject(loginDTO));
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
                await _brokerService.SendToLog(_response, "RefreshToken",
                    ((UserDTO)_response.Result).Token);
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Errors = new List<string>() { "Ошибка логина или пароля" };
            return Unauthorized(_response);
        }

        [HttpPost]
        [Route("CheckToken")]
        public async Task<bool> CheckToken([FromBody]IdentityModel _data)
        {
            _logger.LogInformation("CheckToken - " + JsonConvert.SerializeObject(_data));
            return await Task.FromResult(_tokenService.ValidateToken(_data.token));
        }

        [Authorize]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<ResponseDTO>> RefreshToken()
        {
            _logger.LogInformation("RefreshToken - " + ClaimTypes.Name);
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == User.FindFirstValue(ClaimTypes.Name));
            if (user == null) return Unauthorized();
            _response.Result = await _userHelper.CreateUserDTO(user);
            await _brokerService.SendToLog(_response, "RefreshToken", 
                ((UserDTO)_response.Result).Token);
            return Ok(_response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDTO>> Register([FromBody]RegisterDTO registerDTO)
        {
            _logger.LogInformation("Register - " + JsonConvert.SerializeObject(registerDTO));
            if (await _userManager.Users.AnyAsync(p => p.UserName == registerDTO.Login))
            {
                return BadRequest("Логин уже используется. Выберите другой Login");
            }
            var newUser = new ApplicationUser
            {
                UserName = registerDTO.Login,
                DisplayName = registerDTO.DisplayName
            };
            var result = _userManager.CreateAsync(newUser, registerDTO.Password);
            if (result.Result.Succeeded)
            {
                //добавление роли User для нового пользователя
                var userRole = await _roleManager.FindByNameAsync("User");
                if (userRole != null)
                {
                    await _userManager.AddToRoleAsync(newUser, userRole.Name);
                }
                _response.Result = await _userHelper.CreateUserDTO(newUser);
                return Ok(_response);
            }
            var error = "";
            foreach (var er in result.Result.Errors)
            {
                error += er.Description + ", ";
            }
            _response.IsSuccess = false;
            _response.Errors = new List<string>() { "Ошибка создания нового пользователя - " + error };
            return BadRequest(_response);
        }
    }
}
