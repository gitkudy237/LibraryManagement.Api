using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.AppUserDtos
{
    public class MemberRegistrationDto
    {
        [Required]
        [StringLength(55)]
        public required string Name { get; set; }


        [Required]
        [StringLength(55)]
        public required string Phone { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        [StringLength(55)]
        public required string Password { get; set; }
    }
}