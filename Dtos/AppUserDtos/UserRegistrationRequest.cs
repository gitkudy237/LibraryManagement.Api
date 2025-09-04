namespace LibraryManagement.Dtos.AppUserDtos
{
    public class UserRegistrationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}