namespace LibraryManagement.Dtos.BookDtos;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies{ get; set; }
    public int AuthorId { get; set; }
}
