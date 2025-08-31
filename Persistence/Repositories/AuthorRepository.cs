using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Extensions;
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

    public async Task<QueryResult<Author>> GetAllAsync(AuthorQueryObject authorQueryObj)
    {
        var queryResult = new QueryResult<Author>();
        var authors = _context.Authors
            .Include(a => a.Books)
            .AsQueryable();

        authors = authors.ApplyFiltering(authorQueryObj);
        authors = authors.ApplySorting(authorQueryObj);

        queryResult.TotalItems = await authors.CountAsync();
        authors = authors.ApplyPagination(authorQueryObj);

        queryResult.Items = await authors.ToListAsync();

        return queryResult;
    }

    public void Delete(Author author)
    {
        _context.Authors.Remove(author);
    }
}
