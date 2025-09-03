namespace LibraryManagement.Core.Models;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public ICollection<Borrowing> Borrowings { get; set; } = [];
}
