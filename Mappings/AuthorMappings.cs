using LibraryManagement.Dtos.AuthorDtos;
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

    public static AuthorDto ToAuthorDto(this Author authorModel)
    {
        return new AuthorDto
        {
            Id = authorModel.Id,
            Name = authorModel.Name,
            Bio = authorModel.Bio,
            DateOfBirth = authorModel.DateOfBirth,
        };
    }

    public static void MapUpdateAuthor(this Author authorModel, UpdateAuthorDto updateAuthorDto)
    {
        authorModel.Name = updateAuthorDto.Name;
        authorModel.Bio = updateAuthorDto.Bio;
        authorModel.DateOfBirth = updateAuthorDto.DateOfBirth;
    }
}
