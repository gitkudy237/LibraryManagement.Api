using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Dtos.QueryResultDtos;
using LibraryManagement.Mappings;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<BookDto>> AddBookAsync(CreateBookDto createBookDto)
        {
            var book = createBookDto.ToBookModel();
            try
            {
                await _unitOfWork.Books.AddAsync(book);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                return OperationResult<BookDto>.Fail($"Error adding book: {ex.Message}", 400);
            }

            return OperationResult<BookDto>.Succeed(book.ToBookDto());
        }

        public async Task<OperationResult> DeleteBookAsync(int id)
        {
            var existingBook = await _unitOfWork.Books.GetByIdAsync(id, includeRelated: true);

            if (existingBook is null)
                return OperationResult.Fail("Book not found", 404);
            if (existingBook.Borrowings.Any(b => b.ReturnDate == null))
                return OperationResult.Fail("Cannot delete actively borrowed book", 400);

            try
            {
                _unitOfWork.Books.Delete(existingBook);
                // await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Fail($"Error deleting book: {ex.Message}");
            }

            return OperationResult.Succeed();

        }

        public async Task<BookQueryResultDto> GetAllBooksAsync(BookQueryObjectDto queryObjDto)
        {
            var bookQueryObj = queryObjDto.ToBookQueryObjectModel();
            var queryResult = await _unitOfWork.Books.GetAll(bookQueryObj);

            return queryResult.ToBookQueryResultDto();
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            return book?.ToBookDto();
        }

        public async Task<OperationResult> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            var existingBook = await _unitOfWork.Books.GetByIdAsync(id);

            if (existingBook is null)
                return OperationResult.Fail("Book not found", 404);

            existingBook.MapUpdateBook(updateBookDto);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Fail($"Error updating book: {ex.Message}");
            }

            return OperationResult.Succeed();
        }
    }
}