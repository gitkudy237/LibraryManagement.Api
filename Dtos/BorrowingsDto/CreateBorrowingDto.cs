using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.BorrowingsDto
{
    public class CreateBorrowingDto
    {
        [Required]
        public int BookId { get; set; }
    }
}