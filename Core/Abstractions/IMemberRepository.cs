using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IMemberRepository
    {
        Task AddAsync(Member borrower);
        Task<Member?> GetByIdAsync(int id, bool includeRelated = false);
        Task<Member?> GetByUserIdAsync(string userId, bool includeRelated = false);
        Task<IEnumerable<Member>> GetAllAsync();
        void Delete(Member borrower);
    }
}