using LibraryManagement.Models;

namespace LibraryManagement.Persistence;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        if (!context.Authors.Any())
        {
            var data = new Author[]
            {
                new Author{Name = "Author1", Bio = "Author1-Bio", DateOfBirth = new DateOnly(1995, 06, 16)},
                new Author{Name = "Author2", Bio = "Author2-Bio", DateOfBirth = new DateOnly(1996, 07, 26)},
                new Author{Name = "Author3", Bio = "Author3-Bio", DateOfBirth = new DateOnly(1990, 12, 6)},
            };

            await context.AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }
}
