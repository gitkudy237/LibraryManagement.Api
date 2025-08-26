using LibraryManagement.Dtos;
using LibraryManagement.Models;

namespace LibraryManagement.Mappings;

public static class AuthorMappings
{
    public static Author ToAuthorModel(this CreateAuthorDto createAuthorDto)
    {
        return new Author
        {
            Name = createAuthorDto.Name,
            Bio = createAuthorDto.Bio,
            DateOfBirth = createAuthorDto.DateOfBirth,
        };
    }
}
