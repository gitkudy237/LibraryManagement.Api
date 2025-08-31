using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(int id, bool includeRelated = false);
        Task<QueryResult<Book>> GetAll(BookQueryObject bookQueryObj);
        void Delete(Book book);
    }
}