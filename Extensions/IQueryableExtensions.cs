using LibraryManagement.Core.Models;

namespace LibraryManagement.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Book> ApplyFiltering(this IQueryable<Book> query, BookQueryObject bookQueryObj)
        {
            // filtering
            if (!string.IsNullOrWhiteSpace(bookQueryObj.Title))
                query = query.Where(b => b.Title.Contains(bookQueryObj.Title));

            if (!string.IsNullOrWhiteSpace(bookQueryObj.AuthorName))
                query = query.Where(b => b.Author.Name.Contains(bookQueryObj.AuthorName));

            return query;
        }
    }
}