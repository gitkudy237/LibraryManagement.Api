using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BorrowerDtos;

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
            Id = borrowerModel.Id,
            Name = borrowerModel.Name,
            Email = borrowerModel.Email,
            Phone = borrowerModel.Phone
        };
    }

    public static void MapUpdateBorrower(this Borrower borrowerModel, UpdateBorrowerDto UpdateBorrowerDto)
    {
        borrowerModel.Name = UpdateBorrowerDto.Name;
        borrowerModel.Email = UpdateBorrowerDto.Email;
        borrowerModel.Phone = UpdateBorrowerDto.Phone;
    }
}
