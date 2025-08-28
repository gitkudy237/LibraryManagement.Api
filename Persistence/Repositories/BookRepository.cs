using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
            => await _context.Books.AddAsync(book);

        public void Delete(Book book)
            => _context.Books.Remove(book);

        public async Task<BookQueryResult> GetAll(BookQueryObject bookQueryObj)
        {
            var queryResult = new BookQueryResult();
            var books = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            books = books.ApplyFiltering(bookQueryObj);
            books = books.ApplySorting(bookQueryObj);
            books = books.ApplyPagination(bookQueryObj);

            queryResult.Items = await books.ToListAsync();
            queryResult.TotalItems = await books.CountAsync();

            return queryResult;
        }

        public async Task<Book?> GetByIdAsync(int id, bool includeRelated = false)
        {
            if (!includeRelated)
                return await _context.Books.FindAsync(id);

            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}