namespace LibraryManagement.Dtos.BorrowingsDto
{
    public class BorrowingStatusDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateOnly BorrowingDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string? Status { get; set; }
    }
}