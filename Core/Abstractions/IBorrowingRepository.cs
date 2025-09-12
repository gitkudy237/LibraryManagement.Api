using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBorrowingRepository
    {
        Task<Borrowing?> GetByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<Borrowing>> GetAllAsync();
        Task<IEnumerable<Borrowing>> GetOverdue();
        Task<IEnumerable<Borrowing>> GetMemberBorrowings(string userId);
        Task AddAsync(Borrowing borrowing);
    }
}