using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDto
    {
        public RegisterDto(string password, string username)
        {
            Password = password;
            Username = username;
        }

        [Required]
        public string Username { get; }
        
        [Required]
        [StringLength(8, MinimumLength = 6)]
        public string Password { get; }
    }
}