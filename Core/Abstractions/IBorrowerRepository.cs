using LibraryManagement.Core.Models;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBorrowerRepository
    {
        Task AddAsync(Borrower borrower);
        Task<Borrower?> GetByIdAsync(int id);
        Task<IEnumerable<Borrower>> GetAllAsync();
        void Delete(Borrower borrower);
    }
}