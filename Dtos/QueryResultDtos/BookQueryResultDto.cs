using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;

namespace LibraryManagement.Dtos.QueryResultDtos
{
    public class BookQueryResultDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<BookDto> Items { get; set; } = [];
    }
}