namespace LibraryManagement.Dtos.AuthorDtos;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}
