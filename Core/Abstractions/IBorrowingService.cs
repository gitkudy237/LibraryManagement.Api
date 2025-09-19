using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowingsDto;

namespace LibraryManagement.Core.Abstractions
{
    public interface IBorrowingService
    {
        Task<IEnumerable<BorrowingWithStatusDto>> GetAllBorrowingsAsync();
        Task<BorrowingWithStatusDto?> GetBorrowingByIdAsync(int id);
        Task<IEnumerable<BorrowingDto>> GetOverdueBorrowingsAsync();
        Task<IEnumerable<BorrowingWithStatusDto>> GetMemberBorrowingsAsync(string userId);
        Task<OperationResult<BorrowingDto>> BorrowBookAsync(string userId, CreateBorrowingDto createBorrowingDto);
        Task<OperationResult> ReturnBookAsync(string userId, int bookId);
    }
}