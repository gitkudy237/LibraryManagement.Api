using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        if (!context.Authors.Any())
        {
            var authors = new Author[]
            {
                new Author{Name = "Author1", Bio = "Author1-Bio", DateOfBirth = new DateOnly(1995, 06, 16)},
                new Author{Name = "Author2", Bio = "Author2-Bio", DateOfBirth = new DateOnly(1996, 07, 26)},
                new Author{Name = "Author3", Bio = "Author3-Bio", DateOfBirth = new DateOnly(1990, 12, 6)},
            };

            await context.Authors.AddRangeAsync(authors);
            await context.SaveChangesAsync();

        }

        if (!context.Books.Any())
        {
            var books = new Book[]
            {
                new Book
                {
                    Title = "Title1", 
                    Isbn = Guid.NewGuid().ToString(), 
                    PublicationYear = 2017, 
                    AvailableCopies = 5, 
                    AuthorId =  context.Authors.FirstOrDefault(a => a.Name == "Author1")!.Id
                },
                new Book
                {
                    Title = "Title2", 
                    Isbn = Guid.NewGuid().ToString(), 
                    PublicationYear = 2019, 
                    AvailableCopies = 8, 
                    AuthorId =  context.Authors.FirstOrDefault(a => a.Name == "Author1")!.Id
                },
                new Book
                {
                    Title = "Title3", 
                    Isbn = Guid.NewGuid().ToString(), 
                    PublicationYear = 2017, 
                    AvailableCopies = 7, 
                    AuthorId =  context.Authors.FirstOrDefault(a => a.Name == "Author2")!.Id
                },

            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();

        }
    }
}
