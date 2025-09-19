using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        public Task<OperationResult<BookDto>> AddBookAsync(CreateBookDto createBookDto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookDto?> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            throw new NotImplementedException();
        }
    }
}