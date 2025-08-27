using LibraryManagement.Dtos.BorrowerDtos;
using LibraryManagement.Models;

namespace LibraryManagement.Mappings;

public static class BorrowerMappings
{
    public static Borrower ToBorrowerModel(this CreateBorrowerDto CreateBorrowerDto)
    {
        return new Borrower
        {
            Name = CreateBorrowerDto.Name,
            Email = CreateBorrowerDto.Email,
            Phone = CreateBorrowerDto.Phone
        };
    }

    public static BorrowerDto ToBorrowerDto(this Borrower borrowerModel)
    {
        return new BorrowerDto
        {
            Name = borrowerModel.Name,
            Email = borrowerModel.Email,
            Phone = borrowerModel.Phone
        };
    }
}
