using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.BorrowingsDto
{
    public class CreateBorrowingDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int BorrowerId { get; set; }

        [Required] // TODO: later remove this property since borrowing date shall be the current date
        public DateOnly BorrowingDate { get; set; }

        [Required]
        public DateOnly DueDate { get; set; }
    }
}