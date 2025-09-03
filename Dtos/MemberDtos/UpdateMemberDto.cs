using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.BorrowerDtos;

public class UpdateMemberDto
{
    [Required]
    [StringLength(55)]
    public required string Name { get; set; }


    [Required]
    [StringLength(55)]
    public required string Phone { get; set; }
}
