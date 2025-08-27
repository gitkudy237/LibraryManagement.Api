using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions;

public interface IAuthorRepository
{
    Task AddAsync(Author author);
    Task<Author?> GetByIdAsync(int id, bool includeRelated = false);
    Task<IEnumerable<Author>> GetAllAsync();
    void Delete(Author author);
    
}
