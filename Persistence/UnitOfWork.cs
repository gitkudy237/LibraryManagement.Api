using LibraryManagement.Core.Abstractions;
using LibraryManagement.Persistence.Repositories;

namespace LibraryManagement.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    public IAuthorRepository Authors { get; }
    public IBookRepository Books { get; }
    public IMemberRepository Members { get; }
    public IBorrowingRepository Borrowings { get; }

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
        Authors = new AuthorRepository(_context);
        Books = new BookRepository(_context);
        Members = new MembersRepository(_context);
        Borrowings = new BorrowingRepository(_context);
    }


    public async Task<int> CommitAsync() =>
        await _context.SaveChangesAsync();
}
