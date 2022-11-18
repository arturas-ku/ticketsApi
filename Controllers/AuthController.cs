using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SupportAPI.Auth;
using SupportAPI.Auth.Model;

namespace SupportAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (user == null)
                return BadRequest("Request invalid.");

            var newUser = _mapper.Map<AppUser>(registerUserDto);

            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createUserResult.Succeeded)
                return BadRequest("Request invalid.");

            await _userManager.AddToRoleAsync(newUser, AppRoles.RegularUser);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest("Email or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("Email or password is invalid.");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

            return Ok(new SuccessfulLoginDto(accessToken));
        }
    }
}