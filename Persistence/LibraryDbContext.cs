using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence;

public class LibraryDbContext : DbContext
{

    public DbSet<Author> Authors{ get; set; }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
        
    }
}
