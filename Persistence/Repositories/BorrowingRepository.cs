using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

// TODO: Include borrowing status of single borrowing
// TODO: Refactor borrowing status logic and make it reusable
// TODO: Update borrowing dtos accordingly (add status field on dto)

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
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await _context.Borrowings.ToListAsync();
        }

        public async Task AddAsync(Borrowing borrowing)
        {
            await _context.Borrowings.AddAsync(borrowing);
        }

        public async Task<IEnumerable<Borrowing>> GetOverdue()
        {
            return await _context.Borrowings
                .Where(b => b.DueDate < DateOnly.FromDateTime(DateTime.Now) &&
                        b.ReturnDate != null)
                .ToListAsync();
        }
    }
}