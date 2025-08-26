using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Models;

namespace LibraryManagement.Mappings;

public static class BookMappings
{
    public static Book ToBookModel(this CreateBookDto createBookDto)
    {
        return new Book
        {
            Title = createBookDto.Title,
            Isbn = createBookDto.Isbn,
            PublicationYear = createBookDto.PublicationYear,
            AvailableCopies = createBookDto.AvailableCopies,
            AuthorId = createBookDto.AuthorId,
        };
    }

    public static BookDto ToBookDto(this Book bookModel)
    {
        return new BookDto
        {
            Id = bookModel.Id,
            Title = bookModel.Title,
            Isbn = bookModel.Isbn,
            PublicationYear = bookModel.PublicationYear,
            AvailableCopies = bookModel.AvailableCopies,
            AuthorId = bookModel.AuthorId,
        };
    }
}
