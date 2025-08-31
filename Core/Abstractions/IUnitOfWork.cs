namespace LibraryManagement.Core.Abstractions;

public interface IUnitOfWork
{
    public IAuthorRepository Authors { get; }
    public IBookRepository Books { get; }
    public IMemberRepository Members { get; }
    public IBorrowingRepository Borrowings { get; }
    Task<int> CommitAsync();
}
