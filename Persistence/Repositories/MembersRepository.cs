using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class MembersRepository : IMemberRepository
    {
        private readonly LibraryDbContext _context;

        public MembersRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Member borrower)
        {
            await _context.Members.AddAsync(borrower);
        }

        public void Delete(Member borrower)
        {
            _context.Members.Remove(borrower);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id, bool includeBorrowings = false)
        {
            if (!includeBorrowings)
                return await _context.Members
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

            return await _context.Members
                .Include(m => m.Borrowings)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Member?> GetByUserIdAsync(string userId, bool includeRelated = false)
        {
            if (!includeRelated)
                return await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

            return await _context.Members
                .Include(m => m.Borrowings)
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}