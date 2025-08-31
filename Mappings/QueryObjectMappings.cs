using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.QueryObjectDto;

namespace LibraryManagement.Mappings;

public static class QueryObjectMappings
{
    public static BookQueryObject ToBookQueryObjectModel(this BookQueryObjectDto bookQueryObjectDto)
    {
        return new BookQueryObject
        {
            Title = bookQueryObjectDto.Title,
            AuthorName = bookQueryObjectDto.AuthorName,
            SortBy = bookQueryObjectDto.SortBy,
            IsSortAscending = bookQueryObjectDto.IsSortAscending,
            PageNumber = bookQueryObjectDto.PageNumber,
            PageSize = bookQueryObjectDto.PageSize,
        };
    }

    public static AuthorQueryObject ToAuthorQueryObjectModel(this AuthorQueryObjectDto authorQueryObjectDto)
    {
        return new AuthorQueryObject
        {
            Name = authorQueryObjectDto.Name,
            SortBy = authorQueryObjectDto.SortBy,
            IsSortAscending = authorQueryObjectDto.IsSortAscending,
            PageNumber = authorQueryObjectDto.PageNumber,
            PageSize = authorQueryObjectDto.PageSize
        };
    }
}
