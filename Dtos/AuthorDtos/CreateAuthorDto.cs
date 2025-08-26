using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.AuthorDtos;

public class CreateAuthorDto
{
    [Required]
    [StringLength(55)]
    public required string Name { get; set; }

    [Required]
    [StringLength(350)]
    public required string Bio { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }
}
