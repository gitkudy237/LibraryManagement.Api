using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowingsDto;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBorrowingService
    {
        Task<IEnumerable<BorrowingStatusDto>> GetAllBorrowingsAsync();

    }
}