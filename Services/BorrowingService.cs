using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowingsDto;
using LibraryManagement.Mappings;

namespace LibraryManagement.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BorrowingStatusDto>> GetAllBorrowingsAsync()
        {
            var borrowings = await _unitOfWork.Borrowings.GetAllAsync();
            return borrowings.Select(b => b.ToBorrowingWithStatusDto());
        }
    }
}