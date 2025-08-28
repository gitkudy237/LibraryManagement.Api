namespace LibraryManagement.Dtos.BorrowingsDto
{
    public class BorrowingDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateOnly BorrowingDate { get; set; }
        public DateOnly DueDate { get; set; }
    }
}