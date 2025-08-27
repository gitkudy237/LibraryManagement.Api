using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(int id);
        IQueryable<Book> GetAll();
        void Delete(Book book);
    }
}