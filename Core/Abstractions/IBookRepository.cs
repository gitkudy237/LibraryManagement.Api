using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(int id);
        IQueryable<Book> GetAll(BookQueryObject bookQueryObj);
        void Delete(Book book);
    }
}