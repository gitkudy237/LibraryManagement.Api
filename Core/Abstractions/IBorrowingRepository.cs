using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBorrowingRepository
    {
        Task<Borrowing?> GetByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<Borrowing>> GetAllAsync();
    }
}