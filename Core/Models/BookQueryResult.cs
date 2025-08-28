
namespace LibraryManagement.Core.Models
{
    public class BookQueryResult
    {
        public int TotalItems { get; set; }
        public IEnumerable<Book> Items { get; set; } = [];
    }
}