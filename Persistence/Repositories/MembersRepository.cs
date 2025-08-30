using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BorrowerRepository : IMemberRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowerRepository(LibraryDbContext context)
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
            return await _context.Members.ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id, bool includeRelated = false)
        {
            if (!includeRelated)
                return await _context.Members.FindAsync(id);

            return await _context.Members
                .Include(b => b.Borrowings)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}