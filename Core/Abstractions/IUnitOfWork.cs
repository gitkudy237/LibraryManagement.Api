namespace LibraryManagement.Core.Abstractions;

public interface IUnitOfWork
{
    Task CompeteAsync();
}
