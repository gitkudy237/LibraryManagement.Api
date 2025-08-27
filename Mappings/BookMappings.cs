using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.BookDtos;

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

    public static void MapUpdateBook(this Book bookModel, UpdateBookDto updateBookDto)
    {
        bookModel.Title = updateBookDto.Title;
        bookModel.Isbn = updateBookDto.Isbn;
        bookModel.PublicationYear = updateBookDto.PublicationYear;
        bookModel.AvailableCopies = updateBookDto.AvailableCopies;
        bookModel.AuthorId = updateBookDto.AuthorId;
    }
}
