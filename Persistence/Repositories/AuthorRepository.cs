using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(Author author)
    {
        await _context.AddAsync(author);
    }

    public async Task<Author?> GetByIdAsync(int id, bool includeRelated = false)
    {
        if (!includeRelated)
            return await _context.Authors.FindAsync(id);

        return await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public void Delete(Author author)
    {
        _context.Authors.Remove(author);
    }
}
