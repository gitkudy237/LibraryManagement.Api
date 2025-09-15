using LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence;

public class LibraryDbContext : DbContext
{

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<Librarian> Librarians { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Isbn)
            .IsUnique();

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany(b => b.Borrowings)
            .HasForeignKey(b => b.BookId);

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.Member)
            .WithMany(b => b.Borrowings)
            .HasForeignKey(b => b.MemberId);
    }
}
