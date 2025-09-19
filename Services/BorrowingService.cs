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

        public async Task<OperationResult<BorrowingDto>> BorrowBookAsync(string userId, CreateBorrowingDto createBorrowingDto)
        {
            var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
            if (member is null)
                return OperationResult<BorrowingDto>.Fail(errorMessage: "Member not found", errorCode: 404);

            var borrowingRequest = createBorrowingDto.ToBorrowingModel(member.Id);

            if (await _unitOfWork.Borrowings.HasMemberBorrowedWithoutReturning(borrowingRequest.BookId, member.Id))
                return OperationResult<BorrowingDto>.Fail(errorMessage: "Member currently has this book", errorCode: 400);

            var book = await _unitOfWork.Books.GetByIdAsync(borrowingRequest.BookId);
            if (book is null)
                return OperationResult<BorrowingDto>.Fail(errorMessage: "Book not found", errorCode: 404);

            if (book.AvailableCopies < 1)
                return OperationResult<BorrowingDto>.Fail(errorMessage: "No available copies", errorCode: 400);

            book.AvailableCopies--;

            await _unitOfWork.Borrowings.AddAsync(borrowingRequest);
            await _unitOfWork.CommitAsync();

            var result = borrowingRequest.ToBorrowingDto();

            return OperationResult<BorrowingDto>.Succeed(result);

        }

        public async Task<IEnumerable<BorrowingWithStatusDto>> GetAllBorrowingsAsync()
        {
            var borrowings = await _unitOfWork.Borrowings.GetAllAsync();

            return borrowings.Select(b => b.ToBorrowingWithStatusDto());
        }

        public async Task<BorrowingWithStatusDto?> GetBorrowingByIdAsync(int id)
        {
            var borrowing = await _unitOfWork.Borrowings.GetByIdAsync(id, includeRelated: true);

            return borrowing?.ToBorrowingWithStatusDto();
        }
        public async Task<IEnumerable<BorrowingWithStatusDto>> GetMemberBorrowingsAsync(string userId)
        {
            var borrowings = await _unitOfWork.Borrowings.GetMemberBorrowings(userId);

            return borrowings.Select(b => b.ToBorrowingWithStatusDto());
        }

        public async Task<IEnumerable<BorrowingDto>> GetOverdueBorrowingsAsync()
        {
            var overdueBorrowings = await _unitOfWork.Borrowings.GetOverdue();
            var result = overdueBorrowings.Select(ob => ob.ToBorrowingDto());

            return result;
        }

        public async Task<OperationResult> ReturnBookAsync(string userId, int bookId)
        {
            var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
            if (member is null)
                return OperationResult.Fail(errorMessage: "Member not found", errorCode: 404);

            var borrowing = await _unitOfWork.Borrowings
                .GetByBookAndMemberAsync(bookId, member.Id, includeRelated: true);

            if (borrowing is null)
                return OperationResult.Fail(errorMessage: "You have not borrowed this book", errorCode: 400);

            if (borrowing.ReturnDate is not null)
                return OperationResult.Fail(errorMessage: "Book already returned", errorCode: 400);

            borrowing.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            borrowing.Book.AvailableCopies++;

            await _unitOfWork.CommitAsync();

            return OperationResult.Succeed();
        }
    }
}