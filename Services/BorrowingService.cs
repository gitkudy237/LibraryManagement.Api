using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowingsDto;

namespace LibraryManagement.Services
{
    public class BorrowingService : IBorrowingService
    {
        public Task<OperationResult<IEnumerable<BorrowingDto>>> GetAllBorrowingsAsync()
        {
            throw new NotImplementedException();
        }
    }
}