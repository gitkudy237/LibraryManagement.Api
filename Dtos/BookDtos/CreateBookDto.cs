using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.BookDtos;

public class CreateBookDto
{
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(55)]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    public int PublicationYear { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be negative")]
    public int AvailableCopies{ get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be negative")]
    public int AuthorId { get; set; }
}
