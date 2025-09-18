using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowingsDto;

namespace LibraryManagement.Mappings
{
    public static class BorrowingMappings
    {
        public static BorrowingDto ToBorrowingDto(this Borrowing borrowingModel)
        {
            return new BorrowingDto
            {
                Id = borrowingModel.Id,
                BookId = borrowingModel.BookId,
                MemberId = borrowingModel.MemberId,
                BorrowingDate = borrowingModel.BorrowingDate,
                DueDate = borrowingModel.DueDate,
                ReturnDate = borrowingModel.ReturnDate,
            };
        }

        public static BorrowingWithStatusDto ToBorrowingWithStatusDto(this Borrowing borrowingModel)
        {
            return new BorrowingWithStatusDto
            {
                Id = borrowingModel.Id,
                BookId = borrowingModel.BookId,
                MemberId = borrowingModel.MemberId,
                BorrowingDate = borrowingModel.BorrowingDate,
                DueDate = borrowingModel.DueDate,
                ReturnDate = borrowingModel.ReturnDate,
                Status = GetBorrowingStatus(borrowingModel).ToString()
            };
        }

        public static Borrowing ToBorrowingModel(this CreateBorrowingDto borrowingDto, int memberId)
        {
            return new Borrowing
            {
                BookId = borrowingDto.BookId,
                MemberId = memberId,
                BorrowingDate = DateOnly.FromDateTime(DateTime.Now),
                // should return book one week after borrowing date
                DueDate = DateOnly.FromDateTime(DateTime.Now).AddDays(7)
            };
        }

        private static BorrowingStatus GetBorrowingStatus(Borrowing borrowing)
        {
            if (borrowing.ReturnDate.HasValue) return BorrowingStatus.Returned;
            if (borrowing.DueDate < DateOnly.FromDateTime(DateTime.Now) &&
                !borrowing.ReturnDate.HasValue) return BorrowingStatus.Overdue;
            return BorrowingStatus.On_loan;
        }
    }
}