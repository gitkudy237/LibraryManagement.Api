using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Dtos.QueryResultDtos;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBookService
    {
        Task<BookQueryResultDto> GetAllBooksAsync(BookQueryObjectDto queryObjDto);
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<OperationResult<BookDto>> AddBookAsync(CreateBookDto createBookDto);
        Task<OperationResult> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
        Task<OperationResult> DeleteBookAsync(int id);
    }
}