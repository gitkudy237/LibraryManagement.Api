namespace LibraryManagement.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }

    public ICollection<Book> Books { get; set; } = [];
}
