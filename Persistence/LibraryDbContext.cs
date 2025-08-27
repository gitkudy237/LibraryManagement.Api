using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence;

public class LibraryDbContext : DbContext
{

    public DbSet<Author> Authors{ get; set; }
    public DbSet<Book> Books{ get; set; }
    public DbSet<Borrower> Borrowers { get; set; }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Isbn)
            .IsUnique();
    }
}
