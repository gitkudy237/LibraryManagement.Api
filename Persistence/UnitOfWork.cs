using LibraryManagement.Core.Abstractions;

namespace LibraryManagement.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
    }


    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
