using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowingRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Borrowing?> GetByIdAsync(int id, bool includeRelated = false)
        {
            if (!includeRelated)
                return await _context.Borrowings.FindAsync(id);

            return await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Borrower)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _context.Borrowings.ToListAsync();
        }
    }
}