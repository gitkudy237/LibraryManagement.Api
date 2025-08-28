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
                DueDate = borrowingModel.DueDate
            };
        }

        public static Borrowing ToBorrowingModel(this BorrowingDto borrowingDto)
        {
            return new Borrowing
            {
                BookId = borrowingDto.BookId,
                BorrowerId = borrowingDto.BorrowerId,
                BorrowingDate = borrowingDto.BorrowingDate,
                DueDate = borrowingDto.DueDate
            };
        }
    }
}