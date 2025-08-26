using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Models;

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
}
