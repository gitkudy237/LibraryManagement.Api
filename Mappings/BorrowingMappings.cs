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
                BorrowerId = borrowingModel.BorrowerId,
                BorrowingDate = borrowingModel.BorrowingDate,
                DueDate = borrowingModel.DueDate,
                ReturnDate = borrowingModel.ReturnDate,
            };
        }

        public static BorrowingStatusDto ToBorrowingWithStatusDto(this Borrowing borrowingModel)
        {
            return new BorrowingStatusDto
            {
                Id = borrowingModel.Id,
                BookId = borrowingModel.BookId,
                BorrowerId = borrowingModel.BorrowerId,
                BorrowingDate = borrowingModel.BorrowingDate,
                DueDate = borrowingModel.DueDate,
                ReturnDate = borrowingModel.ReturnDate,
                Status = GetBorrowingStatus(borrowingModel)
            };
        }

        public static Borrowing ToBorrowingModel(this CreateBorrowingDto borrowingDto)
        {
            return new Borrowing
            {
                BookId = borrowingDto.BookId,
                BorrowerId = borrowingDto.BorrowerId,
                BorrowingDate = borrowingDto.BorrowingDate,
                DueDate = borrowingDto.DueDate
            };
        }

        private static string GetBorrowingStatus(Borrowing borrowing)
        {
            if (borrowing.ReturnDate.HasValue) return "Returned";
            if (borrowing.DueDate < DateOnly.FromDateTime(DateTime.Now) &&
                !borrowing.ReturnDate.HasValue) return "Overdue";
            return "On Loan";
        }
    }
}