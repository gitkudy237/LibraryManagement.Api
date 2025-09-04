using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.AppUserDtos
{
    public class LoginDto
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}