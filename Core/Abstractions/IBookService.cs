using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<OperationResult<BookDto>> AddBookAsync(CreateBookDto createBookDto);
        Task<OperationResult> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
        Task<OperationResult> DeleteBookAsync(int id);
    }
}