namespace LibraryManagement.Core.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateOnly BorrowingDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? ReturnDate { get; set; }


        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;
    }
}