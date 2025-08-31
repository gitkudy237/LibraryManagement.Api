using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.QueryResultDtos;

namespace LibraryManagement.Mappings
{
    public static class QueryResultMappings
    {
        public static BookQueryResultDto ToBookQueryResultDto(this QueryResult<Book> bookQueryResult)
        {
            return new BookQueryResultDto
            {
                TotalItems = bookQueryResult.TotalItems,
                Items = bookQueryResult.Items.Select(b => b.ToBookDto())
            };
        }

        public static AuthorQueryResultDto ToAuthorQueryResultDto(this QueryResult<Author> authorQueryResult)
        {
            return new AuthorQueryResultDto
            {
                TotalItems = authorQueryResult.TotalItems,
                Items = authorQueryResult.Items.Select(a => a.ToAuthorDto())
            };
        }
    }
}