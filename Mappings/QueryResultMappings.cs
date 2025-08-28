using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.QueryResultDtos;

namespace LibraryManagement.Mappings
{
    public static class QueryResultMappings
    {
        public static BookQueryResultDto ToBookQueryResultDto(this BookQueryResult bookQueryResult)
        {
            return new BookQueryResultDto
            {
                TotalItems = bookQueryResult.TotalItems,
                Items = bookQueryResult.Items.Select(b => b.ToBookDto())
            };
        }
    }
}