using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            string username = registerDto.Username.ToLower();
            if (await UserExists(username)) return BadRequest("Username is taken");

            var user = new AppUser { UserName = username };
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto { Username = user.UserName, Token = await _tokenService.CreateToken(user) };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Register(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName.Equals(loginDto.Username));
            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            return new UserDto { Username = user.UserName, Token = await _tokenService.CreateToken(user) };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}