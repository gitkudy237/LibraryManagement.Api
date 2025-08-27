using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowerRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Borrower borrower)
        {
            await _context.Borrowers.AddAsync(borrower);
        }

        public void Delete(Borrower borrower)
        {
            _context.Borrowers.Remove(borrower);
        }

        public async Task<IEnumerable<Borrower>> GetAllAsync()
        {
            return await _context.Borrowers.ToListAsync();
        }

        public async Task<Borrower?> GetByIdAsync(int id)
        {
            return await _context.Borrowers.FindAsync(id);
        }
    }
}