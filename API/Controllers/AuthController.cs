using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            string username = registerDto.Username.ToLower();

            if (await UserExists(username))
            {
                return BadRequest("Username is taken");
            }

            using var hmac = new HMACSHA512();

            byte[] passwordHashBytes = Encoding.UTF8.GetBytes(registerDto.Password);

            var user = new AppUser()
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(passwordHashBytes),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Register(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName.Equals(loginDto.Username));

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            byte[] computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (computeHash.Where((t, i) => t != user.PasswordHash[i]).Any())
            {
                return Unauthorized("Invalid password");
            }

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}